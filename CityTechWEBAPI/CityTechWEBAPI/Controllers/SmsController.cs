using CityTechWEBAPI.Configurations;
using CityTechWEBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityTechWEBAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SmsController : ControllerBase
	{
		private readonly CityTechDevContext _dbContext;
		private readonly ISmsService _smsService;
		public SmsController(ISmsService smsService, CityTechDevContext dbContext)
		{
			_smsService = smsService;
			_dbContext = dbContext;
		}

		[HttpPost]
		public async Task<IActionResult> SendSms([FromBody] SmsRequestDto request)
		{
			
			var otp = GenerateAndSaveOTP(request.PhoneNumber);
			if(otp == "User Not Found")
			{
				return BadRequest("User Not Found");
			}

			bool sent = await _smsService.SendSmsAsync(request.PhoneNumber, otp);

			if (sent)
				return Ok("SMS sent successfully");
			else
				return BadRequest("Failed to send SMS");
		}


		private string GenerateAndSaveOTP(string phonenumber)
		{
			var usr = _dbContext.TblUsers.FirstOrDefault(i => i.Phone == phonenumber);
			if (usr != null)
			{
				Random random = new Random();
			   string otp = random.Next(100000, 999999).ToString();
			
				usr.Otp = otp;
				_dbContext.Update(usr);
				_dbContext.SaveChanges();
				return otp;
			}
			else
			{
				return "User Not Found";
			}
			
		}
	}
}
