using MediaOutdoor_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Diagnostics.Metrics;
using MediaOutdoor_Backend.VMs;
using MediaOutdoor_Backend.Services;

namespace MediaOutdoor_Backend.Controllers
{
    public class UserController : Controller
    {

        private readonly MediaOutdoorContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webRoot;


        public UserController(MediaOutdoorContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webRoot)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webRoot = webRoot;
        }

        private bool IsPrimaryKeyViolation(DbUpdateException ex)
        {
            // Check if the exception indicates a primary key violation
            // This can vary depending on the database provider you're using
            // You may need to adapt this based on your database provider's error codes or messages
            return ex.InnerException is SqlException sqlException && sqlException.Number == 2627;
        }


        #region Profile Setting

        public IActionResult ProfileSetting()
        {
            var logUser = new SessionData(_httpContextAccessor).GetData();
            TblUser userInfo = _context.TblUsers.Where(x => x.UserId == logUser.UserId).FirstOrDefault();
            return View(userInfo);
        }

        public IActionResult UpdateProfileSetting(string oldPassword, string newPassword, IFormFile profilePic)
        {

            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/User/";
            var upload = Path.Combine(webRootPath, exactPath);
            var FileName = "";

            var logUser = new SessionData(_httpContextAccessor).GetData();

            var updatePass = _context.TblUsers.Where(x => x.UserId == logUser.UserId).FirstOrDefault();


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

                        updatePass.ProfilePic = exactPath + FileName;
                        _context.TblUsers.Update(updatePass);
                        _context.SaveChanges();

                        return Json("PicChanged");
                    }
                }
            }


            if (updatePass != null && updatePass.Password == oldPassword)
            {

                if (profilePic != null)
                {
                    if (profilePic.Length > 0)
                    {
                        var extension = Path.GetExtension(profilePic.FileName);
                        FileName = updatePass.UserId + "-" + updatePass.FirstName + "-" + updatePass.SecondName + extension;

                        using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                        {
                            profilePic.CopyTo(filesStream);
                        }
                    }
                }


                updatePass.ProfilePic = exactPath + FileName;
                updatePass.Password = newPassword;
                _context.TblUsers.Update(updatePass);
                _context.SaveChanges();

                return Json(true);
            }

            return Json("Old Password Not Correct");
        }

        #endregion


        #region User Entry
        public IActionResult UserEntry()
        {
            return View();
        }


        public IActionResult SaveUser(int id, string firstName, string secondName, string userName, string password,
                                      string gender, string userType, string userAddress1, string userAddress2,
                                      string country, string city, string state, string postalCode, string contactNo,
                                      string email, IFormFile profilePic, bool activeStatus)
        {
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/User/";
            var upload = Path.Combine(webRootPath, exactPath);
            var FileName = "";

            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblUsers.Max(x => (int?)x.UserId) ?? 0;
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

                            _context.TblUsers.Add(new TblUser
                            {
                                UserId = id,
                                FirstName = firstName,
                                SecondName = secondName,
                                UserName = userName,
                                Password = password,
                                Gender = gender,
                                UserType = userType,
                                UserAddress1 = userAddress1,
                                UserAddress2 = userAddress2,
                                Country = country,
                                City = city,
                                State = state,
                                PostalCode = postalCode,
                                ContactNo = contactNo,
                                Email = email,
                                ProfilePic = exactPath + FileName,
                                ActiveStatus = activeStatus,

                            });

                        }
                        else
                        {
                            var user = _context.TblUsers.Where(x => x.UserId == id).FirstOrDefault();

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
                                FileName = user.ProfilePic;
                                exactPath = "";
                            }

                            if (user != null)
                            {

                                user.FirstName = firstName;
                                user.SecondName = secondName;
                                user.UserName = userName;
                                user.Password = password;
                                user.Gender = gender;
                                user.UserType = userType;
                                user.UserAddress1 = userAddress1;
                                user.UserAddress2 = userAddress2;
                                user.Country = country;
                                user.City = city;
                                user.State = state;
                                user.PostalCode = postalCode;
                                user.ContactNo = contactNo;
                                user.Email = email;
                                user.ProfilePic = exactPath + FileName;
                                user.ActiveStatus = activeStatus;

                                _context.TblUsers.Update(user);
                            }
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        success = true;

                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblUsers.Max(x => (int?)x.UserId) ?? 0;
                            id = maxNumber + 1; ;
                            transaction.Rollback();
                            _context.ChangeTracker.Clear();
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

            return Json(true);
        }

        public IActionResult GetUser()
        {
            return Json(_context.TblUsers.AsNoTracking().OrderBy(o => o.UserId).ToList());
        }

        public IActionResult DelUser(int id, string profilePic)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.TblUsers.Where(x => x.UserId == id).ExecuteDelete();
                    _context.SaveChanges();

                    if (!string.IsNullOrEmpty(profilePic))
                    {
                        var webRootPath = _webRoot.WebRootPath;
                        var imageFilePath = Path.Combine(webRootPath, profilePic);

                        if (System.IO.File.Exists(imageFilePath))
                        {
                            System.IO.File.Delete(imageFilePath);
                        }
                    }

                    transaction.Commit();

                    return Json(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }
        }

        #endregion


        #region Customer

        public IActionResult Customers()
        {
            return View();
        }


        public IActionResult SaveCustomers(int id, string firstName, string secondName, string userName, string password,
                                           string gender, DateTime dob, string userAddress1, string userAddress2,
                                           string country, string city, string state, string postalCode, string contactNo,
                                           string email, IFormFile profilePic, bool activeStatus)
        {
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/Customer/";
            var upload = Path.Combine(webRootPath, exactPath);
            var FileName = "";

            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblCustomers.Max(x => (int?)x.CustomerId) ?? 0;
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

                            _context.TblCustomers.Add(new TblCustomer
                            {
                                CustomerId = id,
                                FirstName = firstName,
                                SecondName = secondName,
                                UserName = userName,
                                Password = password,
                                Gender = gender,
                                Dob = dob,
                                Address1 = userAddress1,
                                Address2 = userAddress2,
                                Country = country,
                                City = city,
                                State = state,
                                PostalCode = postalCode,
                                ContactNo = contactNo,
                                Email = email,
                                ProfilePic = exactPath + FileName,
                                ActiveStatus = activeStatus,
                            });

                        }
                        else
                        {
                            var customer = _context.TblCustomers.Where(x => x.CustomerId == id).FirstOrDefault();

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

                                //customer.FirstName = firstName;
                                //customer.SecondName = secondName;
                                //customer.UserName = userName;
                                //customer.Password = password;
                                //customer.Gender = gender;
                                //customer.Dob = dob;
                                //customer.Address1 = userAddress1;
                                //customer.Address2 = userAddress2;
                                //customer.Country = country;
                                //customer.City = city;
                                //customer.State = state;
                                //customer.PostalCode = postalCode;
                                //customer.ContactNo = contactNo;
                                //customer.Email = email;
                                //customer.ProfilePic = exactPath + FileName;
                                customer.ActiveStatus = activeStatus;

                                _context.TblCustomers.Update(customer);
                            }
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        success = true;

                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblCustomers.Max(x => (int?)x.CustomerId) ?? 0;
                            id = maxNumber + 1; ;
                            transaction.Rollback();
                            _context.ChangeTracker.Clear();
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

            return Json(true);
        }


        public IActionResult GetCustomers()
        {
            return Json(_context.TblCustomers.AsNoTracking().OrderBy(o => o.CustomerId).ToList());
        }

        public IActionResult DelCustomers(int id, string profilePic)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.TblCustomers.Where(x => x.CustomerId == id).ExecuteDelete();
                    _context.SaveChanges();

                    if (!string.IsNullOrEmpty(profilePic))
                    {
                        var webRootPath = _webRoot.WebRootPath;
                        var imageFilePath = Path.Combine(webRootPath, profilePic);

                        if (System.IO.File.Exists(imageFilePath))
                        {
                            System.IO.File.Delete(imageFilePath);
                        }
                    }

                    transaction.Commit();

                    return Json(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }
        }

        #endregion

    }
}
