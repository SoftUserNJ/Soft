using System.Data.SqlClient;
using CityTech.Models;
using CityTech.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Data;

namespace CityTech.Controllers
{
    public class WorkOrderController : Controller
    {
        private readonly CityTechContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webRoot;
        private readonly ILog _log;

        public WorkOrderController(CityTechContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webRoot, ILog log)
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
        [AllowPage]
        public IActionResult WorkOrder()
        {
            ViewData["IsPartial"] = false;
            var qryform = _context.Tblownforms
         .Select(f => new Tblownform
         {
             Formid = f.Formid,
             Formname = f.Formname
         })
         .ToList();



            return View(qryform);
        }


        //After Testing and demo check this if this function is not use then delete this
        //public JsonResult GetCustomerData()
        //{
        //    return Json(_context.TblCustomers.ToList());
        //}

        //public JsonResult GetWorkOrderObjects()
        //{
        //    var customers = _context.TblObjects.Select(x => new {
        //        id = x.ObjectId,
        //        name = x.ObjectName ?? "",
        //    }).ToList();
        //    return Json(customers);
        //}




        public JsonResult FilledForms(int incidentNo)
        {
         


            var filledForms = _context.TblIncownforms
        .Where(x => x.IncidentNo == incidentNo)
        .Select(x => new { id = x.Formid, name = x.FormName })
        .ToList();


            return Json(filledForms);
        }


        public JsonResult GetIncidentTypeDropDowns()
        {
            var prepration = _context.TblPreprations.ToList();
            var requirement = _context.TblRequirements.ToList();
            var prio = _context.TblPrios.ToList();
            var skill = _context.TblSkills.ToList();

            var data = new { prepration, requirement, prio, skill };
            return Json(data);
        }

        public JsonResult GetWorkOrderMechanics()
        {
            var mechanic = (from U in _context.TblUsers
                            join T in _context.TblUserTypes on U.UserTypeId equals T.UserTypeId
                            where (T.UserType.Equals("Mechanic"))
                            select new
                            { id = U.UserId, name = U.UserName }).ToList();
            return Json(mechanic);
        }

        //public JsonResult GetWorkOrderIncidentType()
        //{
        //    var customers = _context.TblIncidentTypes.Select(x => new {
        //        id = x.IncidentTypeId,
        //        name = x.IncidentName ?? "",
        //        prepration = x.Prepration ?? "",
        //        requirements = x.Requirements ?? "",
        //        secureDate = x.Slasecure,
        //        fixedDate = x.Slafixed,
        //    }).ToList();
        //    return Json(customers);
        //}

        //public JsonResult GetWorkAddress(int customerId)
        //{
        //    return Json(_context.TblObjectLocations.Where(c => c.CustomerId.Equals(customerId)).ToList());
        //}

        public string GetWorkOrder()
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry1 = @"SELECT I.INCIDENTNO AS ORDERNO, FORMAT(I.INCIDENTDATE, 'yyyy-MM-ddTHH:mm:ssZ') AS DATE,  FORMAT(I.ASSIGNEDSECURE, 'yyyy-MM-ddTHH:mm:ssZ')  AS SECUREDATE,  FORMAT(I.ASSIGNEDFIXED, 'yyyy-MM-ddTHH:mm:ssZ')    AS FIXEDDATE, 
                            ISNULL(I.WORKDETAIL, '') AS WORKDETAIL, ISNULL(I.IMGPATH, '') AS IMGPATH,
                            ISNULL(I.PREPRATION, '') AS PREPRATION, ISNULL(I.REQUIREMENT, '') AS REQUIREMENT,
                            ISNULL(C.CUSTOMERID, '') AS CUSTOMERID, ISNULL(C.CUSTOMERNAME, '') AS CUSTOMERNAME,
                            ISNULL(C.EMAIL, '') AS CUSTOMEREMAIL, ISNULL(C.PHONE, '') AS CUSTOMERPHONE,
                            ISNULL(ITS.INCIDENTTYPEID, '') AS INCIDENTTYPEID, ISNULL(ITS.INCIDENTNAME, '') AS INCIDENTNAME,
                            ISNULL(O.OBJECTID, '') AS OBJECTID, ISNULL(O.OBJECTNAME, '') AS OBJECTNAME, ISNULL(U.USERID, '') AS USERID,
                            ISNULL(U.USERNAME, '') AS USERNAME, ISNULL(M.USERID, '')AS MECHANICID, ISNULL(M.USERNAME, '') AS MECHANICNAME,
                            ISNULL(OL.LOCNAME,'') AS WORKADDRESS, ISNULL(OL.CONTACTPERSON, '') AS CONTACTPERSON, OL.LOCID AS LOCID, ISNULL(S.STATION,'') AS STATION ,
                            ISNULL(I.OWNFORM,'') OWNFORM
                            FROM TBLINCIDENTS I
                            LEFT OUTER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = I.CUSTOMERID
                            LEFT OUTER JOIN TBLINCIDENTTYPES ITS ON ITS.INCIDENTTYPEID = I.INCIDENTTYPEID
                            LEFT OUTER JOIN TBLOBJECTS O ON O.OBJECTID = I.OBJECTID
                            LEFT OUTER JOIN TBLOBJECTLOCATIONS OL ON I.LOCID = OL.LOCID
                            LEFT OUTER JOIN TBLUSERS U ON U.USERID = I.USERID
                            LEFT OUTER JOIN TBLUSERS M ON M.USERID = I.MECHANICID
                            LEFT OUTER JOIN TBLSTATION S ON S.ID = OL.STATIONID
                            LEFT OUTER JOIN TBLUSERTYPES UT ON UT.USERTYPEID = M.USERTYPEID WHERE I.INCIDENTTAG = 'mannual'  ";

            var dt1 = dl.LoadData(qry1);

            
            //foreach (DataRow row in dt1.Rows)
            //{
         
            //    ConvertDateColumnToUtc(row, "DATE");
            //    ConvertDateColumnToUtc(row, "SECUREDATE");
                
            //}


            var maxNo = _context.TblIncidents.Max(x => (int?)x.IncidentNo) ?? 0;
            var order = new { maxNoUnique = maxNo + 1, workorder = dt1 };

            return JsonConvert.SerializeObject(order);
        }





        private void ConvertDateColumnToUtc(DataRow row, string columnName)
        {
            if (row[columnName] != DBNull.Value && DateTime.TryParse(row[columnName].ToString(), out DateTime dateValue))
            {
                row[columnName] = DateTime.SpecifyKind(dateValue, DateTimeKind.Utc).ToString("o");
            }
            else
            {
                row[columnName] = null; // or some default value
            }
        }


        public IActionResult SaveWorkOrder(int id, int customerId, int locId, string workDetail, IFormFile Image,
                                            DateTime incidentDate, DateTime incidentSecureDate, DateTime incidentFixedDate,
                                            int incidentTypeId, string prepration,
                                            string requirement, int objectId, int mechanicId, DateTime activityLogDateTime, string selectedForms)
        {
            NotificationController notificationController = new NotificationController();

            var MechanicId = _context.TblUsers.AsNoTracking().Where(x => x.UserId.Equals(mechanicId)).FirstOrDefault();
            string fcmToken = MechanicId.FcmToken;


            var logUser = new SessionData(_httpContextAccessor).GetData();
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/WorkOrder/";
            var upload = Path.Combine(webRootPath, exactPath);
            var FileName = "";
            int incidentNo = 0;
            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {



                        // Split the string into an array of strings based on the comma delimiter
                      




                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblIncidents.Max(x => (int?)x.IncidentNo) ?? 0;
                            id = maxNoUnique + 1;

                            if (Image != null)
                            {
                                if (Image.Length > 0)
                                {
                                    var extension = Path.GetExtension(Image.FileName);
                                    FileName = id + "-" + "WorkOrder - " + customerId + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        Image.CopyTo(filesStream);
                                    }
                                }
                            }










                            _context.TblIncidents.Add(new TblIncident
                            {
                                IncidentNo = id,
                                WorkDetail = workDetail,
                                CustomerId = customerId,
                                LocId = locId,
                                IncidentDate = incidentDate,
                                ScheduleDate = incidentDate,
                                AssignedSecure = incidentSecureDate,
                                AssignedFixed = incidentFixedDate,
                                IncidentTypeId = incidentTypeId,
                                IncidentTag = "Mannual",
                                Prepration = prepration,
                                Requirement = requirement,
                                ObjectId = objectId,
                                MechanicId = mechanicId,
                                Emailno = "0",

                                ImgPath = exactPath + FileName,
                                IsScheduled = true,
                                UserId = logUser.UserId,
                                Ownform = string.Join(",", selectedForms),
                            });

                        }
                        else
                        {
                            var order = _context.TblIncidents.Where(x => x.IncidentNo == id).FirstOrDefault();

                            if (Image != null)
                            {
                                if (Image.Length > 0)
                                {
                                    var extension = Path.GetExtension(Image.FileName);
                                    FileName = id + "-" + "WorkOrder - " + customerId + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        Image.CopyTo(filesStream);
                                    }
                                }
                            }
                            else
                            {
                                FileName = order.ImgPath;
                                exactPath = "";
                            }

                            if (order != null)
                            {
                                order.WorkDetail = workDetail;
                                order.CustomerId = customerId;
                                order.LocId = locId;
                                order.ScheduleDate = incidentDate;
                                order.AssignedSecure = incidentSecureDate;
                                order.AssignedFixed = incidentFixedDate;
                                order.IncidentTypeId = incidentTypeId;
                                order.Prepration = prepration;
                                order.Requirement = requirement;
                                order.ObjectId = objectId;
                                order.MechanicId = mechanicId;
                                order.Emailno = "0";
                                order.IncidentTag = "Mannual";

                                order.ImgPath = exactPath + FileName;
                                order.IsScheduled = true;
                                order.UserId = logUser.UserId;
                                order.Ownform = string.Join(",", selectedForms);
                                _context.TblIncidents.Update(order);
                            }

                            

                        }



                        string[] selectedFormsArray = selectedForms.Split(',');

                        // Convert the string array to a list of integers
                        List<int> formNumberList = selectedFormsArray.Select(int.Parse).ToList();
                        var formList = _context.TblIncownforms.Where(x => x.IncidentNo == id).ToList();
                        foreach (var form in formList)
                        {
                            _context.TblIncownforms.Remove(form);
                        }
                        //Again New Form
                        var OwnformList = _context.Tblownforms.Where(x => formNumberList.Contains(x.Formid)).ToList();
                        foreach (var ownform in OwnformList)
                        {

                            var newIncownform = new TblIncownform
                            {
                                IncidentNo = id,
                                Formid = ownform.Formid,
                                FormName = ownform.Formname,
                                FormData = ownform.Formdata
                            };
                            _context.TblIncownforms.Add(newIncownform);
                        }



                        _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Work Order No." + id + " Name: " + workDetail + " Added", id, "", "");



                        _context.SaveChanges();

                        transaction.Commit();
                        TblIncident? updateIndidnet = _context.TblIncidents.Where(x => x.IncidentNo.Equals(id) && x.IsScheduled.Equals(false)).FirstOrDefault();

                        notificationController.SendNotification(fcmToken, id, updateIndidnet, "NEW JOB RECEIVE", "CityTech", "ICONE");

                        success = true;

                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblIncidents.Max(x => (int?)x.IncidentNo) ?? 0;
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

        public JsonResult DelWorkOrder(int id, string image, DateTime activityLogDateTime)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    string workDetail = "";
                    var delWo = _context.TblIncidents.Where(d => d.IncidentNo.Equals(id)).FirstOrDefault();
                    workDetail = delWo.WorkDetail;
                    _context.Remove(delWo);
                    _context.SaveChanges();

                    _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Work Order No." + id + " Name: " + workDetail + " Deleted", 0, "", "");

                    if (!string.IsNullOrEmpty(image))
                    {
                        var webRootPath = _webRoot.WebRootPath;
                        var imageFilePath = Path.Combine(webRootPath, image);

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

    }
}
