using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IUser
    {
        DataTable GetUsersList(string locId);
        string Register(UserVM vM);
        string DeleteUser(int userId);
        DataTable GetCurrentUser();
        string ProfileUpdate(UserProfileVM vM);
    }

    public class Users : IUser
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        private readonly IFileUpload _fileUpload;
        private readonly IWebHostEnvironment _hostingEnvironment;

        readonly AuthVM auth = new();
        public Users(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData, IFileUpload fileUpload, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            _fileUpload = fileUpload;
            _hostingEnvironment = hostingEnvironment;

            auth = _auth.GetUserData();
        }

        public DataTable GetUsersList(string locId)
        {
            string filter = "";
            if (auth.IsSuperAdmin == false)
            {
                filter = "AND IsSuperAdmin <> 1";
            }

            string qry = $@"SELECT CG.GrpId AS GroupId, CG.CompName AS GroupName, C.Cmp_id AS CompanyId, C.Cmp_Name AS CompanyName, L.LocId, L.LocName, U.Id AS UserId, U.UserName, U.Email,
            U.Password, U.Designation , U.Cnic, U.Mobile, U.Type, U.Permission, U.Dashboard, '/Companies/{auth.CmpName}/UserImages/' + U.Image AS Image 
            FROM USERS U 
            INNER JOIN COMPANY C ON U.CMP_ID = C.CMP_ID 
            INNER JOIN COMPANYGROUP CG ON C.GRPID = CG.GRPID 
            INNER JOIN LOCATION L ON L.LOCID = U.LOCID AND U.CMP_ID = L.CMP_ID 
            WHERE U.CMP_ID = {auth.CmpId} {filter} AND U.LOCID LIKE '{locId}' ";

            return _dataLogic.LoadData(qry);
        }

        public string Register(UserVM vM)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (vM.Password != vM.ConfirmPassword)
                {
                    return "Password and confirm password does not match...!";
                }

                var shortName = _context.Companies.Where(x => x.CmpId == vM.CompanyId).Select(y => y.ShortName).FirstOrDefault();

                int userOt = 0;

                bool userAdmin = false;

                if (vM.UserType == "OT")
                {
                    userOt = 1;
                }
                else if (vM.UserType == "Admin")
                {
                    userAdmin = true;
                }

                User user = _context.Users.Where(x => x.Id == vM.UserId && x.CmpId == vM.CompanyId).FirstOrDefault();

                var fileName = "No-image.jpg";

                if (vM.Picture != null)
                {
                    var extension = Path.GetExtension(vM.Picture.FileName);
                    fileName = vM.UserName + extension;
                }
                else
                {
                    if (user != null)
                    {
                        if (user.Image != "No-image.jpg")
                        {
                            fileName = user.Image;
                        }
                    }
                    else
                    {
                        fileName = "No-image.jpg";
                    }
                }

                if (user == null)
                {
                    _context.Users.Add(new User
                    {
                        UserName = vM.UserName,
                        CmpShortName = shortName,
                        Password = vM.Password,
                        Type = vM.UserType,
                        Otid = userOt,
                        Admin = userAdmin,
                        Email = vM.Email,
                        Designation = vM.Designation,
                        Cnic = vM.Cnic,
                        Mobile = vM.Mobile,
                        Permission = vM.Permission,
                        CmpId = vM.CompanyId,
                        LocId = vM.LocationId,
                        Image = fileName,
                        Dashboard = vM.Dashboard,
                        IsSuperAdmin = false,
                        Status = "1",
                    });
                }
                else
                {
                    user.UserName = vM.UserName;
                    user.CmpShortName = shortName;
                    user.Password = vM.Password;
                    user.Type = vM.UserType;
                    user.Otid = userOt;
                    user.Admin = userAdmin;
                    user.Email = vM.Email;
                    user.Designation = vM.Designation;
                    user.Cnic = vM.Cnic;
                    user.Mobile = vM.Mobile;
                    user.Permission = vM.Permission;
                    user.CmpId = vM.CompanyId;
                    user.LocId = vM.LocationId;
                    user.Dashboard = vM.Dashboard;
                    user.Image = fileName;
                    _context.Users.Update(user);
                }

                 // file, comapnyName, FolderName, fileName
                _fileUpload.fileUpload(vM.Picture, auth.CmpName, "UserImages", vM.UserName, _hostingEnvironment);

                _context.SaveChanges();
                transaction.Commit();
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        public string DeleteUser(int userId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Users.Where(x => x.Id == userId && x.CmpId == auth.CmpId).ExecuteDelete();
                _context.SaveChanges();
                transaction.Commit();
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }

        }

        public DataTable GetCurrentUser()
        {
            string qry = @"SELECT * FROM USERS WHERE ID = "+ auth.UserId + "";
            return _dataLogic.LoadData(qry);
        }

        public string ProfileUpdate(UserProfileVM vM)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var fileName = "No-image.jpg";

                User user = _context.Users.Where(x => x.Id == auth.UserId && x.CmpId == auth.CmpId).FirstOrDefault();
                if (user != null)
                {
                    if (vM.OldPass != null)
                    {
                        if (user.Password != vM.OldPass)
                        {
                            return "Old Password is Wrong";
                        }
                    }

                    if (vM.Image != null)
                    {
                        var extension = Path.GetExtension(vM.Image.FileName);
                        fileName = user.UserName + extension;
                    }
                    else
                    {
                        if (user != null)
                        {
                            if (user.Image != "No-image.jpg")
                            {
                                fileName = user.Image;
                            }
                        }
                        else
                        {
                            fileName = "No-image.jpg";
                        }
                    }

                    user.UserName = vM.UserName;
                    user.Email = vM.Email;
                    user.Mobile = vM.Mobile;
                    user.Password = (vM.NewPass == null) ? user.Password : vM.NewPass;
                    user.Image = fileName;

                    _context.Users.Update(user);
                }

                // file, comapnyName, FolderName, fileName
                _fileUpload.fileUpload(vM.Image, auth.CmpName, "UserImages", fileName, _hostingEnvironment);

                _context.SaveChanges();
                transaction.Commit();
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }

        }
    }
}
