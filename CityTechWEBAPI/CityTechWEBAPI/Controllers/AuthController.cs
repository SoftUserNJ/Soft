using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CityTechWEBAPI;
using CityTechWEBAPI.Models;
using CityTechWEBAPI.Models.ViewModel;
using CityTechWEBAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;


[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
	private readonly CityTechDevContext _dbContext;
	private readonly IConfiguration _configuration;
	private readonly Logging _logging;


	public AuthController(IAuthService authService , IUserRepository userRepository, CityTechDevContext dbContext, IConfiguration configuration, Logging logging)
    {
        _authService = authService;
        _userRepository = userRepository;
		_dbContext = dbContext;
		_configuration = configuration;
        _logging = logging;
	}

    [HttpGet("login")]
    
    public async Task<string> Login(string UserName, string Password , string FCMToken, DateTime LogDate, string Activity, string Lat, string Long)
    {
        TblUser user = await _userRepository.GetUserByUsername(UserName, Password);
		if (user.UserTypeId == 3)
		{
			if (user != null)
			{
				bool isTokenUnique = !_dbContext.TblUsers.Any(u => u.FcmToken == FCMToken);

				if (!isTokenUnique)
				{
					var userWithExistingToken = await _dbContext.TblUsers.SingleOrDefaultAsync(u => u.FcmToken == FCMToken);
					if (userWithExistingToken != null)
					{
						userWithExistingToken.FcmToken = null; 
						_dbContext.Update(userWithExistingToken);
						await _dbContext.SaveChangesAsync();
					}
				}

				user.FcmToken = FCMToken;
				await _dbContext.SaveChangesAsync();

				user.AccessToken = GetAccessToken(user);
				_dbContext.Update(user);

				var log = new TblLog
				{
					Tag = "app",
					Activity = Activity,
					Latitude = Lat,
					Longitude = Long,
					LogDate = LogDate,
					UserId = user.UserId

				};

				await _dbContext.TblLogs.AddAsync(log);

				await _dbContext.SaveChangesAsync();

                var ProfilePicture = Path.Combine("http://citytechdev.softaxe.com/",user.ImgPath);
				var user1 = new
				{
					MechanicId = user.UserId,
					UserName = user.UserName,
					Password = user.Password,
					UserTypeId = user.UserTypeId,
					SkillId = user.SkillId,
					FcmToken = user.FcmToken,
					AccessToken = user.AccessToken,
					ProfilePicture = ProfilePicture
				};

				

				string json = JsonConvert.SerializeObject(user1);
				return json;
			}
		}
        
        // Handle the case when the user is not found
        return JsonConvert.SerializeObject(  Enumerable.Empty<TblUser>());
    }



	private string GetAccessToken(TblUser user)
	{
		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
			new Claim("UserId", user.UserId.ToString()),
			new Claim("DisplayName", user.FirstName),
			new Claim("UserName", user.UserName),
			new Claim("Email", user.Email),
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
		var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var token = new JwtSecurityToken(
			_configuration["Jwt:Issuer"],
			_configuration["Jwt:Audience"],
			claims,
			expires: DateTime.UtcNow.AddDays(1),
			signingCredentials: signIn
		);

		string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
		return tokenString;
	}

	

	[HttpGet("GetSkills")]
	public async Task<IActionResult> GetSkills()
	{
		try
		{
			List<TblSkill> skills = await _dbContext.TblSkills.ToListAsync();

			if (skills == null || skills.Count == 0)
			{
				return NotFound("No Skills found.");
			}

			return Ok(skills);
		}
		catch (Exception ex)
		{
			return StatusCode(500, "An error occurred while Retreiving the Skills.");
		}

	}

	[HttpPost("LogOut")]
	public async Task<IActionResult> LogOut(int userId, DateTime LogDate, string Activity, int IncidentNo, string Latitude, string Longitude)
    {
        var user = await _dbContext.TblUsers.FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            return NotFound();
        }

        user.AccessToken = null;

        try
        {
            await _dbContext.SaveChangesAsync();
			await _logging.ProcessLog(userId, LogDate, Activity, IncidentNo, Latitude, Longitude);
            return Ok("LogOut Successful"); 
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPost("SignUp")]
	public IActionResult SignUp(List<UserSkills> UserSkills, string firstName, string secondName, string email, string phone, string address, string Username, string password, string gender, string emrgncyContact)
	{
		using (var transaction = _dbContext.Database.BeginTransaction())
		{

			try
			{
				
				 int	id = _dbContext.TblUsers.Max(x => x.UserId) + 1;

					foreach (var item in UserSkills)
					{
						_dbContext.TblUserSkills.Add(new TblUserSkill { UserId = id, SkillId = item.SkillId, Active = item.Active });
					}

					_dbContext.TblUsers.Add(new TblUser
					{
						UserId = id,
						FirstName = firstName,
						SecondName = secondName,
						Email = email,
						Phone = phone,
						Address = address,
						UserName = Username,
						Password = password,
						Gender = gender,
						SkillId = 0,
						EmergencyNo = emrgncyContact
					});

				_dbContext.SaveChanges();

				transaction.Commit();

				return Ok("User Added successfully.");
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return StatusCode(500, "An error occurred while updating the password.");
			}
		}
	}


	[HttpPost("VerifyOtp")]
	public async Task<IActionResult> VerifyOtp(string VrfctionMethod, string otp, string NewPassword, string? Phone = null, string? email = null)
	{ 
		if(VrfctionMethod == "EmailVerification")
		{
			var user = await _dbContext.TblUsers.FirstOrDefaultAsync(i => i.Email == email);

			if (user == null)
			{
				return NotFound();
			}

			if (user.Otp != otp)
			{
				return BadRequest("Invalid OTP");
			}

			user.Password = NewPassword;
		}

		if(VrfctionMethod == "SmsVerification")
		{
			var user =  await _dbContext.TblUsers.FirstOrDefaultAsync(i => i.Phone == Phone);

			if (user == null)
			{
				return NotFound();
			}

			if (user.Otp != otp)
			{
				return BadRequest("Invalid OTP");
			}

			user.Password = NewPassword;
		}
		

		try
		{
			await _dbContext.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{

			return StatusCode(500, "An error occurred while updating the password.");
		}


		return Ok("Password updated successfully!");
	}

}


