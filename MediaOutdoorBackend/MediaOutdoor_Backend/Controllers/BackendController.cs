using MediaOutdoor_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaOutdoor_Backend.Controllers
{
    public class BackendController : Controller
    {

        private readonly MediaOutdoorContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webRoot;

        public BackendController(MediaOutdoorContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webRoot)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webRoot = webRoot;
        }

        [UserAuthentication]
        public IActionResult Index()
        {
            return View();
        }

        #region Log In

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userName, string one, string two, string three, string four, string five, string six)
        {
            var pass = one + two + three + four + five + six;

            if (userName == null)
            {
                ViewBag.Error = "Please enter user name";
                return View();
            }
            else if (pass == null)
            {
                ViewBag.Error = "Please enter password";
                return View();
            }

            var user = _context.TblUsers.Where(x => x.UserName.ToLower() == userName.ToLower() && x.Password == pass && x.ActiveStatus == true).FirstOrDefault();

            if (user == null)
            {
                ViewBag.Error = "User Name or Password is Incorrect";
                return View();
            }

            HttpContext.Session.SetString("UserId", Convert.ToString(user.UserId));
            HttpContext.Session.SetString("Password", Convert.ToString(user.Password));
            HttpContext.Session.SetString("UserName", Convert.ToString(user.UserName));
            HttpContext.Session.SetString("FirstName", Convert.ToString(user.FirstName));
            HttpContext.Session.SetString("SecondName", Convert.ToString(user.SecondName));
            HttpContext.Session.SetString("Gender", Convert.ToString(user.Gender));
            HttpContext.Session.SetString("UserType", Convert.ToString(user.UserType));

            return RedirectToAction("Index");
        }

        #endregion


        #region Logout

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Password");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("FirstName");
            HttpContext.Session.Remove("SecondName");
            HttpContext.Session.Remove("Gender");
            HttpContext.Session.Remove("UserType");

            return RedirectToAction("Login");
        }

        #endregion

    }
}
