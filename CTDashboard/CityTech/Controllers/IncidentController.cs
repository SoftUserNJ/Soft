using CityTech.Models;
using CityTech.Sevices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Transactions;

namespace CityTech.Controllers
{
    public class IncidentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CityTechContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILog _log;
        public IncidentController(CityTechContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILog log)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _log = log;
        }

        private bool IsPrimaryKeyViolation(DbUpdateException ex)
        {
            // Check if the exception indicates a primary key violation
            // This can vary depending on the database provider you're using
            // You may need to adapt this based on your database provider's error codes or messages
            return ex.InnerException is SqlException sqlException && sqlException.Number == 2627;
        }

        public JsonResult Locations()
        {
            return Json(_context.TblObjectLocations.ToList());
        }

        public JsonResult Objects(int locId)
        {
            return Json(_context.TblObjects.Where(l => l.LocId == locId).Select(s => new { s.ObjectId, s.ObjectName }).ToList());
        }

        #region Emergency Users

        public JsonResult GetEmContactUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var emc = _context.TblUsers
                      .Where(x => !string.IsNullOrEmpty(x.EmergencyNo) && string.Compare(x.UserName, username) > 0)
                      .OrderBy(x => x.EmergencyNo)
                      .Select(u => u.UserName)
                      .FirstOrDefault();

                if (string.IsNullOrEmpty(emc))
                {
                    var emContact = _context.TblUsers
                     .Where(x => !string.IsNullOrEmpty(x.EmergencyNo))
                     .OrderBy(x => x.EmergencyNo)
                     .Select(u => u.UserName)
                     .FirstOrDefault();

                    return Json(emContact);
                }

                return Json(emc);
            }
            else
            {
                var emc = _context.TblUsers
                     .Where(x => !string.IsNullOrEmpty(x.EmergencyNo))
                     .OrderBy(x => x.EmergencyNo)
                     .Select(u => u.UserName)
                     .FirstOrDefault();

                return Json(emc);
            }
        }

        #endregion

        #region Incident Alert

        public string IncidentGetData()
        {
          
            DataLogic dl = new DataLogic(_configuration);
                String qry1 = @"SELECT ITYPES.INCIDENTNAME AS INCIDENT, I.INCIDENTNO, L.LOCNAME AS LOCATION,
                            ITYPES.PREPRATION, ITYPES.REQUIREMENTS, O.OBJECTNAME AS OBJECT, ITYPES.SLARESPONSE,
                            ITYPES.SLASECURE, ITYPES.SLAFIXED, ITYPES.PRIOTYPE             
                            FROM TBLINCIDENTS I
                            INNER JOIN TBLINCIDENTTYPES ITYPES ON ITYPES.INCIDENTTYPEID = I.INCIDENTTYPEID
                            INNER JOIN TBLOBJECTS O ON O.OBJECTID = I.OBJECTID
                            INNER JOIN TBLOBJECTLOCATIONS L ON L.LOCID = O.LOCID
                            WHERE I.ISSCHEDULED = 'FALSE'";

            String qry2 = @"SELECT U.USERID, U.USERNAME AS MECHANIC, INC.INCIDENTNO 
                            FROM TBLUSERS U 
                            INNER JOIN TBLUSERTYPES UT ON UT.USERTYPEID = U.USERTYPEID
                            INNER JOIN TBLINCIDENTTYPES I ON I.SKILLID = U.SKILLID
                            INNER JOIN TBLINCIDENTS INC ON INC.INCIDENTTYPEID = I.INCIDENTTYPEID
                            WHERE UT.USERTYPE = 'MECHANIC'  AND INC.INCIDENTNO IN ( SELECT INCIDENTNO FROM TBLINCIDENTS   WHERE    ISSCHEDULED = 'FALSE')";

            var dt1 = dl.LoadData(qry1);
            var dt2 = dl.LoadData(qry2);

            var incidentData = new
            {
                Incident = dt1,
                Mechanic = dt2
            };

            return JsonConvert.SerializeObject(incidentData);
        }

        public IActionResult ScheduleIncidentSave(int incidentNo, DateTime scheduleDate, DateTime secureDate, DateTime fixDate, int mechanic, string prepration, string requirement , string[] selectedForms)
        {

   
            var MechanicId = _context.TblUsers.AsNoTracking().Where(x => x.UserId.Equals(mechanic)).FirstOrDefault();
            string fcmToken = MechanicId.FcmToken;
            var SessionData = new SessionData(_httpContextAccessor).GetData();
            NotificationController notificationController = new NotificationController();
             using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (incidentNo != 0)
                    {


                        string[] selectedFormsNumbers = selectedForms;
                        List<int> formNumberList = selectedFormsNumbers.Select(int.Parse).ToList();
                        var formList = _context.TblIncownforms.Where(x =>  x.IncidentNo == incidentNo).ToList();
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
                                IncidentNo = incidentNo,  
                                Formid = ownform.Formid,
                                FormName = ownform.Formname,
                                FormData = ownform.Formdata
                            };
                            _context.TblIncownforms.Add(newIncownform);
                        }













                        TblIncident? updateIndidnet = _context.TblIncidents.Where(x => x.IncidentNo.Equals(incidentNo) && x.IsScheduled.Equals(false)).FirstOrDefault();
                      


                        if (updateIndidnet==null) { return Json(true); }
                        updateIndidnet.ScheduleDate = scheduleDate;
                        updateIndidnet.AssignedSecure = secureDate;
                        updateIndidnet.AssignedFixed = fixDate;
                        updateIndidnet.MechanicId = mechanic;
                        updateIndidnet.IsScheduled = true;
                        updateIndidnet.IncidentTag = "Auto";
                        updateIndidnet.Prepration = prepration;
                        updateIndidnet.Requirement = requirement;
                        updateIndidnet.UserId = SessionData.UserId;
                        _context.TblIncidents.Update(updateIndidnet);
                        _context.SaveChanges();
                        transaction.Commit();
                        notificationController.SendNotification(fcmToken, incidentNo, updateIndidnet, "NEW JOB RECEIVE", "CityTech", "ICONE");
                        string logDate = scheduleDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        _log.LogEntry(logDate, "Incident Scheduled", incidentNo, "", "");
                        return Json(true);
                    }
                    else
                    {
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

        #endregion

        #region Incident Types
        [AllowPage]
        public IActionResult IncidentTypes()
        {
            return View();
        }

        public string GetIncidentTypes()
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry2 = @"SELECT I.INCIDENTTYPEID, I.INCIDENTNAME, I.PREPRATION, I.REQUIREMENTS,
                            I.PRIOTYPE, I.SLARESPONSE, I.SLASECURE, I.SLAFIXED,  S.SKILLID, S.SKILLNAME
                            FROM TBLINCIDENTTYPES I 
                            INNER JOIN TBLSKILLS S ON S.SKILLID = I.SKILLID ORDER BY I.INCIDENTTYPEID";
            var dt2 = dl.LoadData(qry2);
            return JsonConvert.SerializeObject(dt2);
        }

        public JsonResult GetSkills()
        {
            return Json(_context.TblSkills.ToList());
        }

        public IActionResult SaveIncidentTypes(int id, string name, int skillId, string prepration, string requirements, string priotype, int slaresponse, int slasecure, int slafixed, DateTime activityLogDateTime)
        {
            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if(priotype == "0") { priotype = ""; }

                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblIncidentTypes.Max(x => (int?)x.IncidentTypeId) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblIncidentTypes.Add(new TblIncidentType
                            {
                                IncidentTypeId = id,
                                IncidentName = name,
                                SkillId = skillId,
                                Prepration = prepration,
                                Requirements = requirements,
                                PrioType = priotype,
                                Slaresponse = slaresponse,
                                Slasecure = slasecure,
                                Slafixed = slafixed
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Incident Type No."+id+" Name: "+ name+" Added", 0, "", "");

                        }
                        else
                        {
                            var iTypes = _context.TblIncidentTypes.Where(x => x.IncidentTypeId == id).FirstOrDefault();
                            iTypes.IncidentName = name;
                            iTypes.SkillId = skillId;
                            iTypes.Prepration = prepration;
                            iTypes.Requirements = requirements;
                            iTypes.PrioType = priotype;
                            iTypes.Slaresponse = slaresponse;
                            iTypes.Slasecure = slasecure;
                            iTypes.Slafixed = slafixed;
                            _context.TblIncidentTypes.Update(iTypes);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Incident Type No."+id+" Name: "+ name+" Edited", 0, "", "");

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

        public IActionResult DelIncidentTypes(int id, DateTime activityLogDateTime)
        {

            bool idCheck =
                        _context.TblIncidents.Any(x => x.IncidentTypeId.Equals(id));

            if (idCheck)
            {
                return Json("Already In Use");
            }

            string name = "";
            var delType = _context.TblIncidentTypes.Where(d => d.IncidentTypeId.Equals(id)).FirstOrDefault();
            name = delType.IncidentName;
            _context.Remove(delType);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Incident Type No."+id+" Name: "+ name+" Deleted", 0, "", "");

            return Json(true);
        }

        #endregion

        #region Incidents Detail Report

        [AllowPage]
        public IActionResult IncidentsDetailReport()
        {
            return View();
        }

        public string IncidentDetailsList(string fromDate, string toDate)
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry1 = @"SELECT INC.INCIDENTNO AS INCNO, INC.INCIDENTDATE AS INCDATE, OBJ.OBJECTNAME AS OBJECTNAME,
                            OL.LOCNAME AS LOCATION, ITP.INCIDENTNAME AS INCTYPE, U.FIRSTNAME+' '+U.SECONDNAME AS MECHANIC,
                            C.CUSTOMERNAME AS CUSTOMER, INC.ASSIGNEDFIXED AS FIXEDDATE, INC.ASSIGNEDSECURE AS SECUREDATE,
                            INC.WORKEND AS WORKDONEDATE,isnull(INC.ISSCHEDULED,'false') AS ISSCHEDULED, isnull(INC.WORKDONE,'false') AS WORKDONE,
                            isnull(INC.ACCEPTED,'false') AS ACCEPTED, INC.REJECTED AS REJECTED, Isnull(ITP.SLAFIXED,0) SLAFIXED    
                            FROM TBLINCIDENTS INC
                            INNER JOIN TBLOBJECTS OBJ ON OBJ.OBJECTID = INC.OBJECTID
                            INNER JOIN TBLOBJECTLOCATIONS OL ON OBJ.LOCID = OL.LOCID
                            INNER JOIN TBLINCIDENTTYPES ITP ON ITP.INCIDENTTYPEID = INC.INCIDENTTYPEID
                            INNER JOIN TBLUSERS U ON U.USERID = INC.MECHANICID
                            INNER JOIN TBLUSERTYPES UT ON UT.USERTYPEID = U.USERTYPEID
                            INNER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = OBJ.CUSTOMERID 
                            WHERE CONVERT(VARCHAR(11),INC.INCIDENTDATE,111) BETWEEN '" + fromDate + "' AND '" + toDate + "'";
            
            var dt1 = dl.LoadData(qry1);

            return JsonConvert.SerializeObject(dt1);
        }
        [AllowPage]
        public IActionResult Meldingen()
        {
            return View();
        }
        [AllowPage]
        public IActionResult Mijin()
        {
            return View();
        }

        public string MijinIncidentDetailsList()
        {
            var logUser = new SessionData(_httpContextAccessor).GetData();
            DataLogic dl = new DataLogic(_configuration);
            String qry1 = @"SELECT INC.INCIDENTNO AS INCNO, INC.INCIDENTDATE AS INCDATE, OBJ.OBJECTNAME AS OBJECTNAME,
                            OL.LOCNAME AS LOCATION, ITP.INCIDENTNAME AS INCTYPE, U.FIRSTNAME+' '+U.SECONDNAME AS MECHANIC,
                            C.CUSTOMERNAME AS CUSTOMER, INC.ASSIGNEDFIXED AS FIXEDDATE, INC.ASSIGNEDSECURE AS SECUREDATE,
                            INC.WORKDONEAT AS WORKDONEDATE,isnull(INC.ISSCHEDULED,'false') AS ISSCHEDULED, isnull(INC.WORKDONE,'false') AS WORKDONE,
                            isnull(INC.ACCEPTED,'false') AS ACCEPTED, INC.REJECTED AS REJECTED
                            FROM TBLINCIDENTS INC
                            INNER JOIN TBLOBJECTS OBJ ON OBJ.OBJECTID = INC.OBJECTID
                            INNER JOIN TBLOBJECTLOCATIONS OL ON OBJ.LOCID = OL.LOCID
                            INNER JOIN TBLINCIDENTTYPES ITP ON ITP.INCIDENTTYPEID = INC.INCIDENTTYPEID
                            INNER JOIN TBLUSERS U ON U.USERID = INC.MECHANICID
                            INNER JOIN TBLUSERTYPES UT ON UT.USERTYPEID = U.USERTYPEID
                            INNER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = OBJ.CUSTOMERID
                            WHERE INC.USERID = " + logUser.UserId + "";

            var dt1 = dl.LoadData(qry1);

            return JsonConvert.SerializeObject(dt1);
        }

        #endregion

    }
}
