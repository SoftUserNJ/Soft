using System.Data.SqlClient;
using CityTech.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using CityTech.VMs;
using CityTech.Sevices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Linq;

namespace CityTech.Controllers
{
    public class UserController : Controller
    {
        private readonly CityTechContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webRoot;
        private readonly ILog _log;


        public UserController(CityTechContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webRoot, ILog log)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webRoot = webRoot;
            _log = log;
        }

        private bool IsPrimaryKeyViolation(DbUpdateException ex)
        {
            // Check if the exception indicates a primary key violation
            // This can vary depending on the database provider you're using
            // You may need to adapt this based on your database provider's error codes or messages
            return ex.InnerException is SqlException sqlException && sqlException.Number == 2627;
        }

        #region User

        [AllowPage]
        public IActionResult UserAdd()
        {
            return View();
        }

        public class EmergencyContactValues { public string? emContact { get; set; } }

        public IActionResult GetUserData(List<EmergencyContactValues> assignedValues, string emContactEdit)
        {
            var userTypes = _context.TblUserTypes.ToList().OrderBy(x => x.UserTypeId);
            var skills = _context.TblSkills.ToList().OrderBy(x => x.SkillId);

            int mechanic = _context.TblUserTypes
                            .Where(x => x.UserType == "Mechanic")
                            .Select(x => (int?)x.UserTypeId)
                            .FirstOrDefault() ?? 0;

            var assignedContacts = assignedValues.Select(av => av.emContact).ToList();

            //var filteredUsers = _context.TblUsers.Where(x => x.UserTypeId != mechanic && (string.IsNullOrEmpty(x.EmergencyNo) ||
            //                    !assignedContacts.Contains(x.EmergencyNo))).ToList();

            var filteredUsers = _context.TblUsers.Where(x => x.UserTypeId != mechanic).ToList();

            var countRoles = filteredUsers.Select((x, index) => index + 1).Where(x => !assignedContacts.Contains(x.ToString())).ToList();

            if (!string.IsNullOrEmpty(emContactEdit))
            {
                int emContactValue = int.Parse(emContactEdit);
                countRoles.Add(emContactValue);
                countRoles.Sort();
            }

            var userData = new
            {
                UserTypes = userTypes,
                Skills = skills,
                countRoles = countRoles
            };

            return Json(userData);
        }


        public JsonResult EditUserSkills(int userId)
        {
            var skill = (from US in _context.TblUserSkills
                         join S in _context.TblSkills on US.SkillId equals S.SkillId
                         join U in _context.TblUsers on US.UserId equals U.UserId
                         where (U.UserId == userId)
                         select new
                         {
                             skillId = US.SkillId,
                             active = US.Active
                         }).ToList();

            return Json(skill);
        }

        public IActionResult GetUsersList()
        {
            var dbuser = (from U in _context.TblUsers
                          join S in _context.TblSkills on U.SkillId equals S.SkillId into skillGroup
                          from S in skillGroup.DefaultIfEmpty()
                          join T in _context.TblUserTypes on U.UserTypeId equals T.UserTypeId
                          select new
                          {
                              id = U.UserId,
                              name = U.UserName ?? "",
                              password = U.Password ?? "",
                              type = T.UserType ?? "",
                              typeId = T.UserTypeId.ToString() ?? "",
                              skill = S.SkillName ?? "",
                              skillId = S.SkillId.ToString() ?? "",
                              firstName = U.FirstName ?? "",
                              secondName = U.SecondName ?? "",
                              email = U.Email ?? "",
                              phone = U.Phone ?? "",
                              address = U.Address ?? "",
                              gender = U.Gender ?? "",
                              emContact = U.EmergencyNo ?? "",
                              appaccess = U.AppAccess ?? false,
                              dashboardaccess = U.DashboardAccess ?? false,
                              alert = U.ReceiveIncAlert ?? false,
                              isactive = U.IsActive ?? false,
                              imgPath = U.ImgPath ?? "",
                              allowCall = U.AllowCall ?? false,
                              allowSms = U.AllowSms ?? false,
                              allowWhatsapp = U.AllowWhatsapp ?? false,
                              allowEmail = U.AllowEmail ?? false,
                              inTime = U.InTime,
                              outTime = U.OutTime,

                          }).ToList();
            return Json(dbuser);
        }


        public IActionResult SaveUser(int id, string firstName, string secondName, string email,
                                    string phone, string address, string name, string password,
                                    string gender, int userTypeId, bool dashboardAccess, bool appAccess,
                                    bool incidentAlertReceived, int skillId, string emContact,
                                    bool allowCall, bool allowSms, bool allowWhatsapp, bool allowEmail, bool isactive,
                                    DateTime inTime, DateTime outTime, IFormFile Image, DateTime activityLogDateTime)
        {
            var skillsJson = HttpContext.Request.Form["Skills"];
            var skills = JsonConvert.DeserializeObject<List<UserSkills>>(skillsJson);

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

                            if (Image != null)
                            {
                                if (Image.Length > 0)
                                {
                                    var extension = Path.GetExtension(Image.FileName);
                                    FileName = id + "-" + name + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        Image.CopyTo(filesStream);
                                    }
                                }
                            }

                            foreach (var item in skills)
                            {
                                _context.TblUserSkills.Add(new TblUserSkill { UserId = id, SkillId = item.SkillId, Active = item.Active });
                            }

                            //_context.TblSecurities.Add(new TblSecurity { UserId = id, Url = "/Home/CityTechIndex" });

                            _context.TblUsers.Add(new TblUser
                            {
                                UserId = id,
                                FirstName = firstName,
                                SecondName = secondName,
                                Email = email,
                                Phone = phone,
                                Address = address,
                                UserName = name,
                                Password = password,
                                Gender = gender,
                                UserTypeId = userTypeId,
                                SkillId = skillId,
                                EmergencyNo = emContact,
                                AllowCall = allowCall,
                                AllowEmail = allowEmail,
                                AllowWhatsapp = allowWhatsapp,
                                AllowSms = allowSms,
                                AppAccess = appAccess,
                                DashboardAccess = dashboardAccess,
                                ReceiveIncAlert = incidentAlertReceived,
                                InTime = inTime,
                                OutTime = outTime,
                                ImgPath = exactPath + FileName,
                                IsActive= isactive

                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "User No." + id + " Name: " + name + " Added", 0, "", "");

                        }
                        else
                        {
                            var user = _context.TblUsers.Where(x => x.UserId == id).FirstOrDefault();

                            if (Image != null)
                            {
                                if (Image.Length > 0)
                                {
                                    var extension = Path.GetExtension(Image.FileName);
                                    FileName = id + "-" + name + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        Image.CopyTo(filesStream);
                                    }
                                }
                            }
                            else
                            {
                                FileName = user.ImgPath;
                                exactPath = "";
                            }

                            // User Skill Update Start

                            _context.TblUserSkills.Where(u => u.UserId.Equals(id)).ExecuteDelete();

                            foreach (var item in skills)
                            {
                                _context.TblUserSkills.Add(new TblUserSkill { UserId = id, SkillId = item.SkillId, Active = item.Active });
                            }
                            // User Skill Update End

                            if (user != null)
                            {
                                user.FirstName = firstName;
                                user.SecondName = secondName;
                                user.Email = email;
                                user.Phone = phone;
                                user.Address = address;
                                user.UserName = name;
                                user.Password = password;
                                user.Gender = gender;
                                user.UserTypeId = userTypeId;
                                user.SkillId = skillId;
                                user.EmergencyNo = emContact;
                                user.AllowCall = allowCall;
                                user.AllowSms = allowSms;
                                user.AllowWhatsapp = allowWhatsapp;
                                user.AllowEmail = allowEmail;
                                user.AppAccess = appAccess;
                                user.DashboardAccess = dashboardAccess;
                                user.ReceiveIncAlert = incidentAlertReceived;
                                user.InTime = inTime;
                                user.OutTime = outTime;
                                user.ImgPath = exactPath + FileName;
                                user.IsActive = isactive;

                                _context.TblUsers.Update(user);
                            }
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "User No." + id + " Name: " + name + "  Edited", 0, "", "");
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

        public IActionResult DelUser(int id, string imgName, DateTime activityLogDateTime)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    bool idCheck =
                        _context.TblIncidentWorks.Any(x => x.MechanicId.Equals(id)) ||
                        _context.TblIncidentWorks.Any(x => x.UserId.Equals(id)) ||
                        _context.TblIncidents.Any(x => x.MechanicId.Equals(id)) ||
                        _context.TblIncidents.Any(x => x.UserId.Equals(id));

                    if (idCheck)
                    {
                        return Json("Already In Use");
                    }

                    string name = "";
                    var delUser = _context.TblUsers.Where(x => x.UserId == id).FirstOrDefault();
                    name = delUser.UserName;
                    _context.Remove(delUser);
                    _context.SaveChanges();
                    _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "User No." + id + " Name: " + name + "  Deleted", 0, "", "");


                    if (!string.IsNullOrEmpty(imgName))
                    {
                        var webRootPath = _webRoot.WebRootPath;
                        var imageFilePath = Path.Combine(webRootPath, imgName);

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

        #region Types
        [AllowPage]
        public IActionResult UserTypes()
        {
            return View();
        }

        public IActionResult SaveTypes(int id, string name, DateTime activityLogDateTime)
        {
            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblUserTypes.Max(x => (int?)x.UserTypeId) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblUserTypes.Add(new TblUserType { UserTypeId = id, UserType = name });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "User Type No." + id + " Name: " + name + " Added", 0, "", "");

                        }
                        else
                        {
                            var type = _context.TblUserTypes.Where(x => x.UserTypeId == id).FirstOrDefault();
                            type.UserType = name;
                            _context.TblUserTypes.Update(type);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "User Type No." + id + " Name: " + name + " Edited", 0, "", "");

                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        success = true;

                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblUserTypes.Max(x => (int?)x.UserTypeId) ?? 0;
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

        public IActionResult DelTypes(int id, string name, DateTime activityLogDateTime)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Remove(_context.TblUserTypes.Where(x => x.UserTypeId == id && x.UserType == name).FirstOrDefault());
                    _context.SaveChanges();
                    _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "User Type No." + id + " Name: " + name + " Deleted", 0, "", "");

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

        #region Skill
        [AllowPage]
        public IActionResult UserSkills()
        {
            return View();
        }

        public IActionResult SaveSkill(int id, string name, DateTime activityLogDateTime)
        {
            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblSkills.Max(x => (int?)x.SkillId) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblSkills.Add(new TblSkill { SkillId = id, SkillName = name });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "User Skill No." + id + " Name: " + name + " Added", 0, "", "");

                        }
                        else
                        {
                            var skill = _context.TblSkills.Where(x => x.SkillId == id).FirstOrDefault();
                            skill.SkillName = name;
                            _context.TblSkills.Update(skill);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "User Skill No." + id + " Name: " + name + " Edited", 0, "", "");

                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        success = true;

                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblSkills.Max(x => (int?)x.SkillId) ?? 0;
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

        public IActionResult DelSkill(int id, DateTime activityLogDateTime)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    bool idCheck =
                        _context.TblUserSkills.Any(x => x.SkillId.Equals(id)) ||
                        _context.TblIncidentTypes.Any(x => x.SkillId.Equals(id));

                    if (idCheck)
                    {
                        return Json("Already In Use");
                    }

                    string name = "";
                    var delS = _context.TblSkills.Where(x => x.SkillId == id).FirstOrDefault();
                    name = delS.SkillName;
                    _context.Remove(delS);
                    _context.SaveChanges();
                    _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "User Skill No." + id + " Name: " + name + " Deleted", 0, "", "");


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

        //#region Location

        //public IActionResult SaveLocation(int id, string name)
        //{
        //    bool success = false;
        //    while (!success)
        //    {
        //        using (var transaction = _context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                if (id == 0)
        //                {
        //                    var maxNoUnique = _context.TblObjectLocations.Max(x => (int?)x.LocId) ?? 0;
        //                    id = maxNoUnique + 1;
        //                    _context.TblObjectLocations.Add(new TblObjectLocation { LocId = id, LocName = name });
        //                }
        //                else
        //                {
        //                    var tblObjectLocation = _context.TblObjectLocations.Where(x => x.LocId == id).FirstOrDefault();
        //                    tblObjectLocation.LocName = name;
        //                    _context.TblObjectLocations.Update(tblObjectLocation);
        //                }

        //                _context.SaveChanges();

        //                transaction.Commit();
        //                success = true;
        //            }
        //            catch (DbUpdateException ex)
        //            {
        //                if (IsPrimaryKeyViolation(ex))
        //                {
        //                    int maxNumber = _context.TblObjectLocations.Max(x => (int?)x.LocId) ?? 0;
        //                    id = maxNumber + 1; ;
        //                    transaction.Rollback();
        //                    _context.ChangeTracker.Clear();
        //                }
        //                else
        //                {
        //                    transaction.Rollback();
        //                    return Json(false);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                return Json(false);
        //            }
        //        }
        //    }

        //    return Json(true);
        //}

        //public IActionResult DelLocation(int id)
        //{
        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            _context.Remove(_context.TblObjectLocations.Where(x => x.LocId == id).FirstOrDefault());
        //            _context.SaveChanges();

        //            transaction.Commit();

        //            return Json(true);
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            return Json(false);
        //        }
        //    }
        //}

        //#endregion

        #region Customer
        [AllowPage]
        public IActionResult CustomerAdd()
        {
            return View();
        }

        public IActionResult CustomerGet()
        {
            var customers = _context.TblCustomers.Select(x => new {
                id = x.CustomerId,
                name = x.CustomerName ?? "",
                email = x.Email ?? "",
                phone = x.Phone ?? "",
                gender = x.Gender ?? "",
                business = x.BusinessName ?? "",
                vatno = x.VatNo ?? "",
                commercechamber = x.ChamberOfCommerceNo ?? "",
            }).OrderBy(o => o.name).ToList();
            return Json(customers);
        }

        public IActionResult CustomerSave(int id, string name, string phone, string email, string gender, string businessName, string vatNo, string ChamberOfCommerceNo, DateTime activityLogDateTime)
        {
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

                            _context.TblCustomers.Add(new TblCustomer
                            {
                                CustomerId = id,
                                CustomerName = name,
                                Phone = phone,
                                Email = email,
                                Gender = gender,
                                BusinessName = businessName,
                                VatNo = vatNo,
                                ChamberOfCommerceNo = ChamberOfCommerceNo,
                            });

                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Customer No." + id + " Name: " + name + " Added", 0, "", "");
                        }
                        else
                        {
                            var customer = _context.TblCustomers.Where(x => x.CustomerId == id).FirstOrDefault();
                            if (customer != null)
                            {
                                customer.CustomerName = name;
                                customer.Phone = phone;
                                customer.Email = email;
                                customer.Gender = gender;
                                customer.BusinessName = businessName;
                                customer.VatNo = vatNo;
                                customer.ChamberOfCommerceNo = ChamberOfCommerceNo;

                                _context.TblCustomers.Update(customer);

                                _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Customer No." + id + " Name: " + name + " Edited", 0, "", "");

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

        public IActionResult DelCustomer(int id, DateTime activityLogDateTime)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    bool idCheck =
                        _context.TblObjects.Any(x => x.CustomerId.Equals(id)) ||
                        _context.TblObjectLocations.Any(x => x.CustomerId.Equals(id)) ||
                        _context.TblIncidents.Any(x => x.CustomerId.Equals(id));

                    if (idCheck)
                    {
                        return Json("Already In Use");
                    }

                    string name = "";
                    var delC = _context.TblCustomers.Where(x => x.CustomerId == id).FirstOrDefault();
                    name = delC.CustomerName;

                    _context.Remove(delC);
                    _context.SaveChanges();
                    _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Customer No." + id + " Name: " + name + " Deleted", 0, "", "");


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

        #region User Profile Setting

        public IActionResult ProfileSetting()
        {
            var logUser = new SessionData(_httpContextAccessor).GetData();
            TblUser userInfo = _context.TblUsers.Where(x => x.UserId == logUser.UserId).FirstOrDefault();
            return View(userInfo);
        }

        public IActionResult UpdateProfileSetting(int id, string name, string oldPassword, string newPassword, DateTime activityLogDateTime, IFormFile profilePic)
        {
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/User/";
            var upload = Path.Combine(webRootPath, exactPath);
            var FileName = "";

            string gender = "";
            var updatePass = _context.TblUsers.Where(x => x.UserId == id).FirstOrDefault();
            if (updatePass.Gender != null)
            {
                if (updatePass.Gender.ToString().ToLower() == "female")
                {
                    gender = "Her";
                }
                else if (updatePass.Gender.ToString().ToLower() == "male")
                {

                    gender = "His";
                }
            }
            else
            {
                gender = "Her/His";
            }

            if (updatePass.Password == oldPassword)
            {

                if (profilePic != null)
                {
                    if (profilePic.Length > 0)
                    {
                        var extension = Path.GetExtension(profilePic.FileName);
                        FileName = id + "-" + name + extension;
                        using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                        {
                            profilePic.CopyTo(filesStream);
                        }
                    }
                }


                updatePass.ImgPath = exactPath + FileName;

                updatePass.Password = newPassword;
                _context.TblUsers.Update(updatePass);
                _context.SaveChanges();
                _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Changed " + gender + " Password", 0, "", "");

                return Json(true);
            }

            return Json("Old Password Not Correct");
        }

        #endregion

        #region Security

        [AllowPage]
        public IActionResult UserRights()
        {
            return View();
        }

        public JsonResult UserPageRights(string url)
        {
            int uid = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserId"));

            var rights = (from R in _context.TblSecurities
                          where R.Url.Equals(url) && R.UserId.Equals(uid)
                          select new
                          {
                              Save = R.Save ?? false,
                              Edit = R.Edit ?? false,
                              Delete = R.Delete ?? false,
                              PDF = R.Pdf ?? false,
                              Excel = R.Excel ?? false,
                              Word = R.Word ?? false,
                          }).FirstOrDefault();

            return Json(rights);
        }

        public JsonResult GetSecurityUsers()
        {
            //var users = (from U in _context.TblUsers
            //             join T in _context.TblUserTypes on U.UserTypeId equals T.UserTypeId
            //             where(U.DashboardAccess == true)
            //             select new
            //             {
            //                 id = U.UserId,
            //                 username = U.UserName,
            //                 usertype = T.UserType

            //             }).ToList();

            //return Json(users);

            return Json(_context.TblUsers.Where(x => x.DashboardAccess == true).Select(s => new { id = s.UserId, username = s.UserName }).ToList());
        }

        public IActionResult SecurityUpdate(List<AllowForm> GridData, int userId, DateTime activityLogDateTime)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblSecurities.Where(x => x.UserId.Equals(userId)).ExecuteDelete();

                if (GridData.Count > 0)
                {
                    foreach (var item in GridData)
                    {
                        _context.TblSecurities.Add(new TblSecurity
                        {
                            MenuId = item.MenuId,
                            MenuName = item.MenuName,
                            Url = item.Url,
                            UserId = userId,
                            Save = item.Save,
                            Edit = item.Edit,
                            Delete = item.Delete,
                            Pdf = item.PDF,
                            Excel = item.Excel,
                            Csv = item.Csv,
                            Word = item.Word,
                        });
                    }
                }
                //_context.TblSecurities.Add(new TblSecurity { UserId = userId, Url = "/Home/CityTechIndex" });

                _context.SaveChanges();

                var userName = _context.TblUsers.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
                _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Changed User Rights of User No." + userId + " Name: " + userName.UserName, 0, "", "");

                transaction.Commit();

                return Json(true);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Json(false);
            }
        }

        public IActionResult GetAllowFoarms(int userId)
        {
            var allowForms = _context.TblSecurities.Where(x => x.UserId == userId).ToList();
            //var Dashboard = _context.TblUsers.Where(x => x.UserId == userId).Select(x => x.Dashboard).FirstOrDefault();

            dynamic getAllLevels = new System.Dynamic.ExpandoObject();
            getAllLevels.Forms = allowForms;
            //getAllLevels.Dashboard = Dashboard;

            return Json(getAllLevels);
        }

        public IActionResult GetAllowMenu()
        {
            var loginUser = new SessionData(_httpContextAccessor).GetData();

            return Json(_context.TblSecurities.Where(x => x.UserId == loginUser.UserId).ToList());
        }
        #endregion
    }

}
