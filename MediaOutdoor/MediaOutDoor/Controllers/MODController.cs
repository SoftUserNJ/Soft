using CityTechWEBAPI;
using MediaOutdoor.Models;
using MediaOutDoor.Models;
using MediaOutDoor.Services;
using MediaOutDoor.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace MediaOutDoor.Controllers
{
    public class MODController : Controller
    {
        private readonly MediaOutdoorContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IEmail _email;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webRoot;


        public MODController(MediaOutdoorContext dbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IEmail email, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webRoot)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _email = email;
            _httpContextAccessor = httpContextAccessor;
            _webRoot = webRoot;
        }


        private bool IsPrimaryKeyViolation(DbUpdateException ex)
        {
            // Check if the exception indicates a primary key violation
            // This can vary depending on the database provider you're using
            // You may need to adapt this based on your database provider's error codes or messages
            return ex.InnerException is SqlException sqlException && sqlException.Number == 2627;
        }


        public IActionResult Index()
        {

            List<TblSlider> sliders = _dbContext.TblSliders.ToList();
            return View(sliders);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        public IActionResult BookingMV()
        {
            return View();
        }

        #region Contact Us

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(string name, string email, string message)
        {
            if (name == null || email == null || message == null)
            {
                ViewBag.Error = "Please fill out all required fields";
                return View();
            }
            
            ViewBag.Success = "Your message has been sent successfully";

            _email.SendEmail(email, "", name + " Drop a message", message, "text");


            return View();
        }

        #endregion

        #region Log In

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SignIn(string email, string password)
        {

            if (email == null)
            {
                ViewBag.Error = "Please enter email";
                return View();
            }
            else if (password == null)
            {
                ViewBag.Error = "Please enter password";
                return View();
            }

            var user = _dbContext.TblCustomers.Where(x => x.Email.ToLower() == email.ToLower() && x.Password == password && x.ActiveStatus == true).FirstOrDefault();

            if (user == null)
            {
                ViewBag.Error = "User Name or Password is Incorrect";
                return View();
            }

            HttpContext.Session.SetString("UserId", Convert.ToString(user.CustomerId));
            HttpContext.Session.SetString("UserName", Convert.ToString(user.UserName));
            HttpContext.Session.SetString("FirstName", Convert.ToString(user.FirstName));
            HttpContext.Session.SetString("SecondName", Convert.ToString(user.SecondName));
            HttpContext.Session.SetString("Email", Convert.ToString(user.Email));
            HttpContext.Session.SetString("Type", Convert.ToString(user.Type?? ""));
            HttpContext.Session.SetString("Pas", Convert.ToString(user.Password));

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("FirstName");
            HttpContext.Session.Remove("SecondName");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Type");
            HttpContext.Session.Remove("Pas");

            return RedirectToAction("Index");
        }

        #endregion

        #region Register

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult SaveCustomer(int id, string firstName, string secondName, string userName, string email,
                                        string contactNo, DateTime dob, string gender, string country, string city,
                                        string state, string postalCode, string address1, string address2,
                                        string password, string sendM, IFormFile profilePic, string visitorId)
        {

            bool usernamecheck = _dbContext.TblCustomers.Any(x => x.UserName.ToLower() == userName.ToLower());
            if (usernamecheck)
            {
                return Json("Kindly choose another username as the selected one is already in use.");
            }

            bool mailcheck = _dbContext.TblCustomers.Any(x => x.Email.ToLower() == email.ToLower());
            if (mailcheck)
            {
                return Json("Kindly choose another email as the selected one is already in use.");
            }

                
            Random rand = new Random();
            var vCode = rand.Next(100000, 999999);


            var webRootPath = _webHostEnvironment.WebRootPath;
            var exactPath = "Images/Customer/";
            var upload = Path.Combine(webRootPath, exactPath);
            var FileName = "";

            bool success = false;
            while (!success)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            var maxNoUnique = _dbContext.TblCustomers.Max(x => (int?)x.CustomerId) ?? 0;
                            id = maxNoUnique + 1;

                            if (profilePic != null)
                            {
                                if (profilePic.Length > 0)
                                {
                                    var extension = Path.GetExtension(profilePic.FileName);
                                    FileName = id + "-" + firstName + "-" + secondName + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        profilePic.CopyTo(filesStream);
                                    }
                                }
                            }

                            _dbContext.TblCustomers.Add(new TblCustomer
                            {
                                CustomerId = id,
                                FirstName = firstName,
                                SecondName = secondName,
                                UserName = userName.ToLower(),
                                Password = password,
                                Gender = gender,
                                Dob = dob,
                                Address1 = address1,
                                Address2 = address2,
                                Country = country,
                                City = city,
                                State = state,
                                PostalCode = postalCode,
                                ContactNo = contactNo,
                                Email = email.ToLower(),
                                ProfilePic = exactPath + FileName,
                                ActiveStatus = false,
                                Otp = vCode.ToString(),
                            });

                        }
                        else
                        {
                            var customer = _dbContext.TblCustomers.Where(x => x.CustomerId == id).FirstOrDefault();

                            if (profilePic != null)
                            {
                                if (profilePic.Length > 0)
                                {
                                    var extension = Path.GetExtension(profilePic.FileName);
                                    FileName = id + "-" + firstName + "-" + secondName + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        profilePic.CopyTo(filesStream);
                                    }
                                }
                            }
                            else
                            {
                                FileName = customer.ProfilePic;
                                exactPath = "";
                            }

                            if (customer != null)
                            {

                                customer.Password = password;
                                customer.ProfilePic = exactPath + FileName;
                                _dbContext.TblCustomers.Update(customer);
                            }
                        }
                      
                        var ordersToUpdate = _dbContext.TblOrders.Where(o => o.VisitorId == visitorId);
                        var maxOrderNo = _dbContext.TblOrders.Max(x => (int?)x.OrderId) ?? 0;
                        var OrderNo = maxOrderNo + 1;
                        foreach (var order in ordersToUpdate)
                        {   
                            order.OrderNo = "MDOT-" + OrderNo;
                            order.Status = "NEW";
                            order.CustomerId = id;
                        }


                        _dbContext.SaveChanges();

                        transaction.Commit();
                        success = true;



                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _dbContext.TblCustomers.Max(x => (int?)x.CustomerId) ?? 0;
                            id = maxNumber + 1; ;
                            transaction.Rollback();
                            _dbContext.ChangeTracker.Clear();
                        }
                        else
                        {
                            transaction.Rollback();
                            return Json(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(false);
                    }
                }
            }

            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string param = url.ToString() + "/MOD/VerifyAccount?vCode="+ vCode +"&email="+ email;

            sendM = sendM.Replace("SOFTAXEURL", param);
            _email.SendEmail("", email, "Account Verification", sendM, "html");

            return Json(true);
        }


        public IActionResult VerifyAccount(string vCode, string email)
        {
            var verify = _dbContext.TblCustomers.Where(x => x.Otp == vCode && x.Email.ToLower() == email.ToLower()).FirstOrDefault();
            
            if (verify != null)
            {
                verify.ActiveStatus = true;
                _dbContext.Update(verify);
                _dbContext.SaveChanges();

                return RedirectToAction("SignIn");
            }

            return Json("Verification Failed");
        }

        #endregion

        #region Station
        public string GetStations()
        {

            DataLogic dl = new DataLogic(_configuration);
            string qry = "\r\nSelect st.StationName, st.Lat, st.Long, st.Address1, st.Address2, st.StationImage, st.stationid , screensize, case when isnull(screensize,'')='' then 0 else  count(*) end  TotalScreen from TblStations st\r\nLeft Join tblscreens sc On sc.stationid = st.stationid \r\ngroup by st.stationimage, st.StationName, st.Lat, st.Long, st.Address1, st.Address2, st.stationid , screensize\r\n";

            string qry2 = "Select * from tblScreens";
            var dt = dl.LoadData(qry);
            var dt2 = dl.LoadData(qry2);


            var result = new
            {
                Stations = dt,
                Screens = dt2

            };

            return JsonConvert.SerializeObject(result);
        }

        #endregion

        #region Screen


        public string GetScreens(int stationid)
        {
            DataLogic dl = new DataLogic(_configuration);

            string qry = "Select  ScreenId, sc.StationId, ScreenName, ScreenSize, st.StationImage, XPosition, YPosition, Rate, st.StationName , width,height   from tblScreens sc\r\nLeft Join tblStations st on st.stationid = sc.stationid where sc.Stationid ='" + stationid + "'";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }

        #endregion

        #region Slider


        [HttpGet]
        public async Task<IActionResult> GetSliders()
        {
            List<TblSlider> sliders = await _dbContext.TblSliders.ToListAsync();
            foreach (var sliderData in sliders)
            {
                sliderData.Image = Path.Combine("http://mediaoutdoorbackend.softaxe.com/", sliderData.Image);
            }

            return Json(sliders);
        }

        #endregion

        #region ProfileSetting

        [UserAuthentication]
        public IActionResult ProfileSetting()
        {
            var logUser = new SessionData(_httpContextAccessor).GetData();
            var profile = _dbContext.TblCustomers.Where(x => x.CustomerId == logUser.UserId).FirstOrDefault();

            ViewBag.fName = profile.FirstName;
            ViewBag.lName = profile.SecondName;
            ViewBag.uName = profile.UserName;
            ViewBag.email = profile.Email;
            ViewBag.phone = profile.ContactNo;
            ViewBag.DOB = profile.Dob;
            ViewBag.Gender = profile.Gender;
            ViewBag.status = profile.ActiveStatus;
            ViewBag.pic = profile.ProfilePic;
            ViewBag.Country = profile.Country;
            ViewBag.City = profile.City;
            ViewBag.State = profile.State;
            ViewBag.pCode = profile.PostalCode;
            ViewBag.address1 = profile.Address1;
            ViewBag.address2 = profile.Address2;

            return View();
        }

        
        public IActionResult UpdateProfileSetting(string firstName, string secondName,
                                        string contactNo, DateTime dob, string gender, string country, string city,
                                        string state, string postalCode, string address1, string address2,
                                        string oldPassword, string newPassword, IFormFile profilePic)
        {
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/Customer/";
            var upload = Path.Combine(webRootPath, exactPath);
            var FileName = "";

            var logUser = new SessionData(_httpContextAccessor).GetData();

            var updatePass = _dbContext.TblCustomers.Where(x => x.UserName == logUser.UserName && x.CustomerId == logUser.UserId).FirstOrDefault();

            if (oldPassword == null && newPassword == null)
            {
                if (profilePic != null)
                {
                    if (profilePic.Length > 0)
                    {
                        var extension = Path.GetExtension(profilePic.FileName);
                        FileName = logUser.UserId + "-" + logUser.FirstName + "-" + logUser.SecondName + extension;

                        using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                        {
                            profilePic.CopyTo(filesStream);
                        }

                    }
                }
                else
                {
                    exactPath = "";
                    FileName = updatePass.ProfilePic;
                }

                updatePass.FirstName = firstName;
                updatePass.SecondName = secondName;
                updatePass.ContactNo = contactNo;
                updatePass.Dob = dob;
                updatePass.Gender = gender;
                updatePass.Country = country;
                updatePass.City = city;
                updatePass.State = state;
                updatePass.PostalCode = postalCode;
                updatePass.Address1 = address1;
                updatePass.Address2 = address2;

                updatePass.ProfilePic = exactPath + FileName;
                _dbContext.TblCustomers.Update(updatePass);
                _dbContext.SaveChanges();

                return Json(true);

            }


            if (updatePass != null && updatePass.Password == oldPassword)
            {
                if (profilePic != null)
                {
                    if (profilePic.Length > 0)
                    {
                        var extension = Path.GetExtension(profilePic.FileName);
                        FileName = updatePass.CustomerId + "-" + updatePass.FirstName + "-" + updatePass.SecondName + extension;

                        using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                        {
                            profilePic.CopyTo(filesStream);
                        }
                    }
                }

                updatePass.FirstName = firstName;
                updatePass.SecondName = secondName;
                updatePass.ContactNo = contactNo;
                updatePass.Dob = dob;
                updatePass.Gender = gender;
                updatePass.Country = country;
                updatePass.City = city;
                updatePass.State = state;
                updatePass.PostalCode = postalCode;
                updatePass.Address1 = address1;
                updatePass.Address2 = address2;


                updatePass.Password = newPassword;
                updatePass.ProfilePic = exactPath + FileName;
                _dbContext.TblCustomers.Update(updatePass);
                _dbContext.SaveChanges();

                return Json(true);
            }

            return Json("Old Password Not Correct");
        }


        #endregion

        #region Password Recover
        public IActionResult ForgotPassword(string email, string recoverMsg)
        {
            Random rand = new Random();
            var vCode = rand.Next(100000, 999999);

            var mailcheck = _dbContext.TblCustomers.Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefault();

            if (mailcheck != null)
            {
                mailcheck.Otp = vCode.ToString();
                _dbContext.Update(mailcheck);
                _dbContext.SaveChanges();

            }
            else
            {
                return Json("You are not registered");
            }

            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string param = url.ToString() + "/MOD/PassRecovery?vCode=" + vCode + "&email=" + email;

            recoverMsg = recoverMsg.Replace("SOFTAXEURL", param);
            _email.SendEmail("", email, "Password Reset", recoverMsg, "html");

            return Json(true);
        }


        public IActionResult PassRecovery(string vCode, string email)
        {

            var verify = _dbContext.TblCustomers.Where(x => x.Otp == vCode && x.Email.ToLower() == email.ToLower()).FirstOrDefault();

            if (verify != null)
            {
                TempData["vCodePassRecovery"] = vCode;
                TempData["emailPassRecovery"] = email;

                return RedirectToAction("SetPassNew");
                //return RedirectToAction("SetPassNew", new { vcode = vCode, email = email });
            }

            return Json("Password Reset Failed");
        }


        [HttpGet]
        public IActionResult SetPassNew()
        {
            ViewBag.vcode = TempData["vCodePassRecovery"];
            ViewBag.email = TempData["emailPassRecovery"];

            return View();
        }


        [HttpPost]
        public IActionResult SetPassNew(string vCode, string email, string password)
        {
            var verify = _dbContext.TblCustomers.Where(x => x.Otp == vCode && x.Email.ToLower() == email.ToLower()).FirstOrDefault();

            if (verify != null)
            {
                verify.Password = password;
                _dbContext.Update(verify);
                _dbContext.SaveChanges();

                return RedirectToAction("SignIn");
            }
            else
            {
                return View();
            }

        }

        #endregion

    }
}
