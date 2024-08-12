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
using System.Net;
using Microsoft.AspNetCore.Authorization;
using static System.Collections.Specialized.BitVector32;


namespace CityTech.Controllers
{
    public class SettingsController : Controller
    {
        private readonly CityTechContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webRoot;
        private readonly ILog _log;


        public SettingsController(CityTechContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webRoot, ILog log)
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

        #region Settings

        [AllowPage]
        public IActionResult Settings()
        {
            TblSetting setting = _context.TblSettings.FirstOrDefault();
            return View(setting);
        }

        public IActionResult SaveSettings(int callerWaitingTime, int nextEngineerWaitingTime, DateTime activityLogDateTime)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // SQL query strings
            string deleteQuery = "DELETE FROM TblSettings;";
            string insertQuery = "INSERT INTO TblSettings (CallerWait, EngineerCallerWait) VALUES (@CallerWait, @EngineerCallerWait);";

            // Create a connection and a command
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Remove all existing rows
                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                {
                    deleteCommand.ExecuteNonQuery();
                }

                // Insert the new row
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@CallerWait", callerWaitingTime);
                    insertCommand.Parameters.AddWithValue("@EngineerCallerWait", nextEngineerWaitingTime);
                    insertCommand.ExecuteNonQuery();
                }
            }
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Settings Time Update", 0, "", "");


            return Json(true);
        }

        #endregion

        #region Objects Settings
        [AllowPage]
        public IActionResult Objects()
        {
            return View();
        }

        public JsonResult GetObjStations()
        {
            return Json(_context.TblStations.ToList());
        }

        public string GetObjectLocations(int id)
        {
            DataLogic dl = new DataLogic(_configuration);
            string qry = @"SELECT L.LOCID AS ID, L.LOCNAME AS ADDRESS, ISNULL(L.LOCNAME2, '') AS ADDRESS2,
                          S.ID AS STATIONID, S.STATION AS STATION, C.CUSTOMERID AS CUSTOMERID, C.CUSTOMERNAME AS CUSTOMER,
                          ISNULL(L.CONTACTPERSON, '') AS CONTACTPERSON, ISNULL(L.CONTACTPERSONPHONE, '') AS CONTACTPERSONPHONE,
                          ISNULL(L.RESIDENCE, '') AS RESIDENCE, ISNULL(L.REGION, '') AS REGION,
                          ISNULL(L.POSTALCODE, '') AS POSTALCODE, ISNULL(L.LATI, '') AS LATI, ISNULL(L.LONGI, '') AS LONGI
                          
                          FROM TBLOBJECTLOCATIONS L
                          INNER JOIN TBLSTATION S ON S.ID = L.STATIONID
                          INNER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = L.CUSTOMERID  WHERE L.CUSTOMERID = '"+ id + "' ";

            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);

            //return Json(_context.TblObjectLocations.Where(x => x.CustomerId.Equals(id)).ToList());
        }

        public string GetCustomerObjects(int customerId)
        {
            DataLogic dl = new DataLogic(_configuration);
            string qry = @"SELECT O.OBJECTID AS ID, O.OBJECTNAME AS NAME, C.CUSTOMERNAME AS CUSTOMER, C.CUSTOMERID AS CUSTOMERID,
                            L.LOCID AS LOCATIONID, L.LOCNAME AS LOCATION, O.STATIONNAME AS STATION, O.POSTALCODE AS POSTALCODE
                            FROM TBLOBJECTS O
                            INNER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = O.CUSTOMERID
                            INNER JOIN TBLOBJECTLOCATIONS L ON L.LOCID = O.LOCID WHERE O.CUSTOMERID = '"+ customerId + "'";

            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }

        public string GetObjects()
        {
            DataLogic dl = new DataLogic(_configuration);
            string qry = @"SELECT O.OBJECTID AS ID, O.OBJECTNAME AS NAME, C.CUSTOMERNAME AS CUSTOMER, C.CUSTOMERID AS CUSTOMERID,
                            L.LOCID AS LOCATIONID, L.LOCNAME AS LOCATION, O.STATIONNAME AS STATION, O.POSTALCODE AS POSTALCODE
                            FROM TBLOBJECTS O
                            INNER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = O.CUSTOMERID
                            INNER JOIN TBLOBJECTLOCATIONS L ON L.LOCID = O.LOCID";

            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }

        public IActionResult SaveObjects(int id, string name, int customerId, int locId, DateTime activityLogDateTime)
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
                            var maxNoUnique = _context.TblObjects.Max(x => (int?)x.ObjectId) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblObjects.Add(new TblObject
                            {
                                ObjectId = id,
                                ObjectName = name,
                                CustomerId = customerId,
                                LocId = locId,
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Object No."+id+" Name: "+ name + " Added", 0, "", "");

                        }
                        else
                        {
                            var objUpdate = _context.TblObjects.Where(x => x.ObjectId == id).FirstOrDefault();
                            objUpdate.ObjectName = name;
                            objUpdate.CustomerId = customerId;
                            objUpdate.LocId = locId;
                            _context.TblObjects.Update(objUpdate);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Object No."+id+" Name: "+ name + " Edited", 0, "", "");

                        }

                        _context.SaveChanges();

                        transaction.Commit();

                        success = true;
                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblObjects.Max(x => (int?)x.ObjectId) ?? 0;
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

        public IActionResult DelObjects(int id, DateTime activityLogDateTime)
        {
            bool idCheck =
                        _context.Tblobjectsactivities.Any(x => x.Objectid.Equals(id)) ||
                        _context.TblIncidents.Any(x => x.ObjectId.Equals(id));

            if (idCheck)
            {
                return Json("Already In Use");
            }

            string name = "";
            var delO = _context.TblObjects.Where(d => d.ObjectId.Equals(id)).FirstOrDefault();
            name = delO.ObjectName;
            _context.Remove(delO);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Object No."+id+" Name: "+ name + "  Deleted", 0, "", "");


            return Json(true);
        }

        #endregion

        #region Locations Settings
        [AllowPage]
        public IActionResult Locations()
        {
            return View();
        }

        public IActionResult DeviceLocations()
        {
            return View();
        }

        public string GetDeviceLocations()
        {
            DataLogic dl = new DataLogic(_configuration);
            string qry = @"SELECT Distinct L.Lati, L.Longi, L.LocId, ObjectName,LocName, LocPath,case when Isnull(WorkDoneStatus,'')<> 'WorkDone' then 'Issue' else '' end Status
FROM TblObjects O
INNER JOIN TblObjectLocations L ON L.LocId=O.LocId
INNER JOIN TblIncidents I ON I.ObjectId=o.ObjectId
where isnull(WorkDoneStatus,'')<>'WorkDone'


union all

SELECT Distinct L.Lati, L.Longi, L.LocId, ObjectName,LocName,LocPath,'No Issue' Status
FROM TblObjects O
INNER JOIN TblObjectLocations L ON L.LocId=O.LocId
where O.ObjectId not in (SELECT Distinct o.ObjectId
FROM TblObjects O
INNER JOIN TblObjectLocations L ON L.LocId=O.LocId
INNER JOIN TblIncidents I ON I.ObjectId=o.ObjectId
where isnull(WorkDoneStatus,'')<>'WorkDone'
)
ORDER BY OBJECTNAME";


            qry = @"select  l.Lati, l.Longi , L.LocId, ObjectName,LocName, LocPath, 
 (CASE WHEN 
 ISnull(( select count(*) from TblIncidents where ObjectId=Obj.ObjectId and isnull(WorkDoneStatus,'')<>'WorkDone' ),0) >0 THEN 
 'Issue' else 'No Issue' end) Status
from TblObjectLocations L
inner join TblObjects obj on obj.LocId=l.LocId";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }




        public string GetObjLocations()
        {
            DataLogic dl = new DataLogic(_configuration);
            string qry = @"SELECT L.LOCID AS ID, L.LOCNAME AS ADDRESS, ISNULL(L.LOCNAME2, '') AS ADDRESS2,
                          S.ID AS STATIONID, S.STATION AS STATION, C.CUSTOMERID AS CUSTOMERID, C.CUSTOMERNAME AS CUSTOMER,
                          ISNULL(L.CONTACTPERSON, '') AS CONTACTPERSON, ISNULL(L.CONTACTPERSONPHONE, '') AS CONTACTPERSONPHONE,
                          ISNULL(L.RESIDENCE, '') AS RESIDENCE, ISNULL(L.REGION, '') AS REGION,
                          ISNULL(L.POSTALCODE, '') AS POSTALCODE, ISNULL(L.LATI, '') AS LATI, ISNULL(L.LONGI, '') AS LONGI
                          
                          FROM TBLOBJECTLOCATIONS L
                          INNER JOIN TBLSTATION S ON S.ID = L.STATIONID
                          INNER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = L.CUSTOMERID";

            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }


        public IActionResult SaveLocations(int id, string address, string address2, int stationId, int customerId,
                                            string contactPerson, string contactPersonPhone,string residence,
                                            string region, string postalCode, string lati, string longi, DateTime activityLogDateTime)
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
                            var maxNoUnique = _context.TblObjectLocations.Max(x => (int?)x.LocId) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblObjectLocations.Add(new TblObjectLocation
                            {
                                LocId = id,
                                LocName = address,
                                LocName2 = address2,
                                StationId = stationId,
                                CustomerId = customerId,
                                ContactPerson = contactPerson,
                                ContactPersonPhone = contactPersonPhone,
                                Residence = residence,
                                Region = region,
                                PostalCode = postalCode,
                                Lati = lati,
                                Longi = longi,
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Object Location No."+id+" Name: "+ address + " Added", 0, "", "");

                        }
                        else
                        {
                            var locUpdate = _context.TblObjectLocations.Where(x => x.LocId == id).FirstOrDefault();
                            locUpdate.LocName = address;
                            locUpdate.LocName2 = address2;
                            locUpdate.StationId = stationId;
                            locUpdate.CustomerId = customerId;
                            locUpdate.ContactPerson = contactPerson;
                            locUpdate.ContactPersonPhone = contactPersonPhone;
                            locUpdate.Residence = residence;
                            locUpdate.Region = region;
                            locUpdate.PostalCode = postalCode;
                            locUpdate.Region = region;
                            locUpdate.Lati = lati;
                            locUpdate.Longi = longi;
                            _context.TblObjectLocations.Update(locUpdate);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Object Location No."+id+" Name: "+ address + " Edited", 0, "", "");

                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        success = true;
                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblObjectLocations.Max(x => (int?)x.LocId) ?? 0;
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

        public IActionResult DelLocations(int id, DateTime activityLogDateTime)
        {

            bool idCheck =
                        _context.TblObjects.Any(x => x.LocId.Equals(id)) ||
                        _context.TblIncidents.Any(x => x.LocId.Equals(id));

            if (idCheck)
            {
                return Json("Already In Use");
            }

            string address = "";
            var delA = _context.TblObjectLocations.Where(d => d.LocId.Equals(id)).FirstOrDefault();
            address = delA.LocName;
            _context.Remove(delA);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Object Location No."+id+" Name: "+ address + " Deleted", 0, "", "");


            return Json(true);
        }


        

        #endregion

        #region Company
        [AllowPage]
        public IActionResult Company()
        {
            return View();
        }

        public IActionResult GetCompanyList()
        {
            return Json(_context.TblCompnays.ToList());
        }

        public IActionResult SaveCompany(int id, string name, string email, string telephone, string city, string postalcode, string address1, string address2, bool autoPilot, IFormFile Image, DateTime activityLogDateTime)
        {
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/Company/";
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
                            int maxNumber = _context.TblCompnays.Max(x => (int?)x.Id) ?? 0;
                            int newNumber = maxNumber + 1;

                            if (Image != null)
                            {
                                if (Image.Length > 0)
                                {
                                    var extension = Path.GetExtension(Image.FileName);
                                    FileName = newNumber + "-" + name + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        Image.CopyTo(filesStream);
                                    }
                                }
                            }

                            _context.TblCompnays.Add(new TblCompnay
                            {
                                Id = newNumber,
                                Name = name,
                                Email = email,
                                Telephone = telephone,
                                City = city,
                                PostalCode = postalcode,
                                Address1 = address1,
                                Address2 = address2,
                                AutoPilot = autoPilot,
                                ImgPath = exactPath + FileName
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Company  No."+id+" Name: "+ name + " Added", 0, "", "");

                        }
                        else
                        {
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

                            var updateContext = _context.TblCompnays.Where(x => x.Id == id).FirstOrDefault();
                            updateContext.Name = name;
                            updateContext.Email = email;
                            updateContext.Telephone = telephone;
                            updateContext.City = city;
                            updateContext.PostalCode = postalcode;
                            updateContext.Address1 = address1;
                            updateContext.Address2 = address2;
                            updateContext.AutoPilot = autoPilot;
                            updateContext.ImgPath = exactPath + FileName;
                            _context.TblCompnays.Update(updateContext);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Company No."+id+" Name: "+ name + " Edited", 0, "", "");
                        }


                        _context.SaveChanges();
                        transaction.Commit();
                        success = true;
                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblCompnays.Max(x => (int?)x.Id) ?? 0;
                            id = maxNumber + 1;
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


        public IActionResult DelCompany(int id, string imgName, DateTime activityLogDateTime)
        {
            string name = "";
            var delC = _context.TblCompnays.Where(d => d.Id.Equals(id)).FirstOrDefault();
            name = delC.Name;
            _context.Remove(delC);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Company No."+id+" Name: "+ name + " Deleted", 0, "", "");


            if (!string.IsNullOrEmpty(imgName))
            {
                var webRootPath = _webRoot.WebRootPath;
                var imageFilePath = Path.Combine(webRootPath, imgName);

                if (System.IO.File.Exists(imageFilePath))
                {
                    System.IO.File.Delete(imageFilePath);
                }
            }

            return Json(true);
        }

        #endregion

        #region Prio
        [AllowPage]
        public IActionResult Prio()
        {
            return View();
        }

        public IActionResult GetPrio()
        {
            return Json(_context.TblPrios.ToList());
        }

        public IActionResult SavePrio(int id, string prio, DateTime activityLogDateTime)
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
                            var maxNoUnique = _context.TblPrios.Max(x => (int?)x.Id) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblPrios.Add(new TblPrio
                            {
                                Id = id,
                                PrioName = prio
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Prio No."+id+" Name: "+ prio + " Added", 0, "", "");
                        }
                        else
                        {
                            var Update = _context.TblPrios.Where(x => x.Id == id).FirstOrDefault();
                            Update.PrioName = prio;
                            _context.TblPrios.Update(Update);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Prio No."+id+" Name: "+ prio + " Edited", 0, "", "");

                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        success = true;
                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblPrios.Max(x => (int?)x.Id) ?? 0;
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

        public IActionResult DelPrio(int id, DateTime activityLogDateTime)
        {
            string prio = "";
            var delP = _context.TblPrios.Where(d => d.Id.Equals(id)).FirstOrDefault();
            prio = delP.PrioName;
            _context.Remove(delP);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Prio No."+id+" Name: "+ prio + " Deleted", 0, "", "");

            return Json(true);
        }

        #endregion

        #region Prepration
        [AllowPage]
        public IActionResult Prepration()
        {
            return View();
        }

        public IActionResult GetPrepration()
        {
            return Json(_context.TblPreprations.ToList());
        }

        public IActionResult SavePrepration(int id, string prepration, DateTime activityLogDateTime)
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
                            var maxNoUnique = _context.TblPreprations.Max(x => (int?)x.Id) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblPreprations.Add(new TblPrepration
                            {
                                Id = id,
                                Prepration = prepration
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Prepration No."+id+" Name: "+ prepration + " Added", 0, "", "");

                        }
                        else
                        {
                            var Update = _context.TblPreprations.Where(x => x.Id == id).FirstOrDefault();
                            Update.Prepration = prepration;
                            _context.TblPreprations.Update(Update);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Prepration No."+id+" Name: "+ prepration + " Edited", 0, "", "");

                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        success = true;
                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblPreprations.Max(x => (int?)x.Id) ?? 0;
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

        public IActionResult DelPrepration(int id, DateTime activityLogDateTime)
        {
            string prepration = "";
            var delP = _context.TblPreprations.Where(d => d.Id.Equals(id)).FirstOrDefault();
            prepration = delP.Prepration;
            _context.Remove(delP);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Prepration No."+id+" Name: "+ prepration + " Deleted", 0, "", "");

            return Json(true);
        }

        #endregion

        #region Requirements
        [AllowPage]
        public IActionResult Requirement()
        {
            return View();
        }

        public IActionResult GetRequirement()
        {
            return Json(_context.TblRequirements.ToList());
        }

        public IActionResult SaveRequirement(int id, string requirement, DateTime activityLogDateTime)
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
                            var maxNoUnique = _context.TblRequirements.Max(x => (int?)x.Id) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblRequirements.Add(new TblRequirement
                            {
                                Id = id,
                                Requirement = requirement,
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Requirement No."+id+" Name: "+ requirement + " Added", 0, "", "");

                        }
                        else
                        {
                            var Update = _context.TblRequirements.Where(x => x.Id == id).FirstOrDefault();
                            Update.Requirement = requirement;
                            _context.TblRequirements.Update(Update);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Requirement No."+id+" Name: "+ requirement + " Edited", 0, "", "");

                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        success = true;
                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblRequirements.Max(x => (int?)x.Id) ?? 0;
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

        public IActionResult DelRequirement(int id, DateTime activityLogDateTime)
        {
            bool idCheck =
                        _context.TblIncidentTypes.Any(x => x.Prepration.Equals(id));

            if (idCheck)
            {
                return Json("Already In Use");
            }

            string requirement = "";
            var delR = _context.TblRequirements.Where(d => d.Id.Equals(id)).FirstOrDefault();
            requirement = delR.Requirement;
            _context.Remove(delR);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Requirement No."+id+" Name: "+ requirement + " Deleted", 0, "", "");

            return Json(true);
        }

        #endregion

        #region Station
        [AllowPage]
        public IActionResult Station()
        {
            return View();
        }

        public IActionResult GetStation()
        {
            return Json(_context.TblStations.Select(s => new { id = s.Id, station = s.Station }).ToList());
        }

        public IActionResult SaveStation(int id, string station, DateTime activityLogDateTime)
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
                            var maxNoUnique = _context.TblStations.Max(x => (int?)x.Id) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblStations.Add(new TblStation
                            {
                                Id = id,
                                Station = station,
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Station No."+id+" Name: "+ station + " Added", 0, "", "");

                        }
                        else
                        {
                            var Update = _context.TblStations.Where(x => x.Id == id).FirstOrDefault();
                            Update.Station = station;
                            _context.TblStations.Update(Update);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Station No."+id+" Name: "+ station + " Eedited", 0, "", "");

                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        success = true;
                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblStations.Max(x => (int?)x.Id) ?? 0;
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


        public IActionResult DelStation(int id, DateTime activityLogDateTime)
        {
            bool idCheck =
                        _context.TblObjectLocations.Any(x => x.StationId.Equals(id));

            if (idCheck)
            {
                return Json("Already In Use");
            }

            string station = "";
            var delS = _context.TblStations.Where(x => x.Id.Equals(id)).FirstOrDefault();
            station = delS.Station;
            _context.Remove(delS);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Station No."+id+" Name: "+ station + " Deleted", 0, "", "");

            return Json(true);
        }

        #endregion

        public IActionResult UserLiveTracking()
        {
            return View();
        }


        public string TrackUserLocations()
        {

            DataLogic dl = new DataLogic(_configuration);
            string qry = @"IF OBJECT_ID('tempdb..#temptrans') IS NOT NULL DROP TABLE #temptrans
 SELECT U.USERID, ol.LocName, U.UserName , isnull(INCT.IncidentName,'') as IncidentName  ,isnull(INC.IncidentNo,'') as IncidentNo, isnull(INC.IncidentDate, '') as IncidentDate
 into  #temptrans
 FROM TblUsers U
 INNER JOIN TblIncidents INC ON INC.MechanicId=U.UserId AND isnull(workdone, 0)=0 And isnull(Accepted,0)=1 AND IncidentNo = ( 
 SELECT MAX(INCIDENTNO) FROM TblIncidents   WHERE  MechanicId=INC.MechanicId   AND isnull(workdone, 0)=0 And isnull(Accepted,0)=1)
 INNER JOIN  TblIncidentTypes INCT ON INCT.IncidentTypeId=INC.IncidentTypeId
  left outer join tblobjectlocations ol on ol.locid = INC.locid
 WHERE UserTypeId=3
 select U.UserId ,isnull(trans.locname,'') locname , U.Gender, u.UserName, l.Latitude,l.Longitude , isnull(trans.IncidentName,'') as IncidentName, isnull(TRANS.IncidentNo, '') as IncidentNo,isnull(trans.IncidentDate, '') as  IncidentDate   from TblUsers u
 inner join TblUserTypes ut on ut.UserTypeId=u.UserTypeId
 inner join TblLog  l on l.UserId =u.UserId and l.LogId = ( select max(logid) from TblLog where UserId=u.UserId)
 left outer join #temptrans trans on trans.UserId=u.UserId
 where ut.UserTypeId=3
 and isnull(l.Latitude,'') <> ''  and isnull(l.Longitude,'')<>''
 and isnull(l.tag,'')='app'";

            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }

    }
}

