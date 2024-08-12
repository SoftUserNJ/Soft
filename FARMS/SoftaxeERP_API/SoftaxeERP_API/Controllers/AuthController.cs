using DevExpress.XtraRichEdit.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.Services;
using SoftaxeERP_API.VM;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SoftaxeERP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUser _user;
        private readonly IDataLogic _logic;

        public AuthController(ErpSoftaxeContext context, IConfiguration configuration, IUser users, IDataLogic logic)
        {
            _context = context;
            _configuration = configuration;
            _user = users;
            _logic = logic;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password, [FromQuery] DateTime dtNow)
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                return BadRequest();

            var user = await _context.Users.FirstOrDefaultAsync(x => (x.UserName + x.CmpShortName) == username && x.Password == password);

            if (user == null)
                return NotFound(new { msg = "User Not Found" });

            var finId = _context.Tblfinyears.Where(x => x.CompId == user.CmpId).Select(y => y.Finid).SingleOrDefault();
            var location = _context.Locations.Where(x => x.CmpId == user.CmpId && x.LocId == user.LocId).Select(y => new { y.LocName, y.DayCloseTime }).SingleOrDefault();
            var wbsettings = _context.TblWbsettings.Where(x => x.Cmpid == user.CmpId && x.Locid == user.LocId).SingleOrDefault();
            var discodes = _context.TblDiscodes.Where(x => x.CmpId == user.CmpId && x.LocId == user.LocId).FirstOrDefault();


            if (user.IsSuperAdmin == false)
            {
                _context.TblLogs.Add(new TblLog
                {
                    Vdate = dtNow,
                    VhrDate = dtNow,
                    Vchno = 0,
                    Vtype = "Login",
                    PurchaseRate = 0,
                    MaxRate = 0,
                    MinRate = 0,
                    Remraks = "Login",
                    Amount = 0,
                    Uid = user.Id,
                    CmpId = user.CmpId,
                    Locid = user.LocId,
                    Finid = finId,
                });
                _context.SaveChanges();
            }

            var cmp = _context.Companies.Where(x => x.CmpId == user.CmpId).FirstOrDefault();

            string[] canFeed = new string[] {};

            if(cmp.ApprovalSystem == true)
            {
                canFeed = _context.TblUserVchTypes.Where(x => x.Uid == user.Id && x.CanFeed == true).Select(z => z.VchType).ToArray();
            }

            return Ok(new
            {
                msg = "Login Success!",
                token = CreateJWT(user),
                userId = user.Id,
                userName = user.UserName,
                designation = user.Designation,
                userType = user.Type,
                admin = user.Admin,
                superAdmin = user.IsSuperAdmin,
                userImage = user.Image,
                isDashBoard = user.Dashboard,
                cmpId = user.CmpId,
                locId = user.LocId,
                locName = location.LocName,
                finId = finId,
                cmp =  cmp,
                stopEntry = canFeed,
                inlimit = wbsettings?.InLimit ,
                shiftime = location.DayCloseTime
            });
        }

        private string CreateJWT(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName + user.CmpShortName),
                new Claim(ClaimTypes.Role, user.Type),
                new Claim("UserId", user.Id.ToString()),
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(6),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        [HttpPost("logout")]
        public IActionResult Logout(DateTime dtNow)
        {
            _logic.LogEntry(0, "Logout", "Logout", 0, dtNow, 0, 0, 0,dtNow);
            return Ok(new
            {
                msg = "Logout Success!",
            });
        }

        [HttpGet("GetUsersList")]
        public IActionResult GetUsersList(string locId)
        {
            var data = _user.GetUsersList(locId);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdateUser")]
        public IActionResult SaveUpdateUser([FromForm] UserVM vM)
        {
            var result = _user.Register(vM);
            return Ok(result);
        }

        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser(int userId)
        {
            var result = _user.DeleteUser(userId);
            return Ok(result);
        }

        [HttpGet("GetCurrentUser")]
        public IActionResult GetCurrentUser()
        {
            var data = _user.GetCurrentUser();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }
        
        [HttpPost("UpdateProfile")]
        public IActionResult UpdateProfile([FromForm]UserProfileVM vM)
        {
            var result = _user.ProfileUpdate(vM);
            return Ok(result);
        }
    }
}
