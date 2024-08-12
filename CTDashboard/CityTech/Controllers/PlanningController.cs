using CityTech.Models;
using CityTech.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using CityTech.Models.ViewModel;
using System.Data;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Http;
//using ImageMagick;
namespace CityTech.Controllers
{

    public class PlanningController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CityTechContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public PlanningController(ILogger<HomeController> logger, CityTechContext context, IHttpContextAccessor httpContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContext;
            _hostingEnvironment = webHostEnvironment;
            _configuration = configuration;
        }





        public JsonResult Planningtask(DateTime? date)
        {
            DateTime inputDate = date ?? DateTime.Now;
            string dateString = inputDate.ToString("yyyy/MM/dd");


            DataLogic dl = new DataLogic(_configuration);
            string incidentQry = $@"
        SELECT Incidentno, 
               MechanicId AS UserId,
               FORMAT(AssignedSecure, 'dd-MM-yyyy') AS AssignedSecureDate,
               FORMAT(AssignedSecure, 'HH:mm') AS TimeAssignSecure,
               FORMAT(AssignedFixed, 'HH:mm') AS TimeAssignFixed, WorkDetail , 

 FORMAT(ASSIGNEDSECURE, 'yyyy-MM-ddTHH:mm:ssZ')  AS SECUREDATE,  FORMAT(ASSIGNEDFIXED, 'yyyy-MM-ddTHH:mm:ssZ')    AS FIXEDDATE
        FROM TblIncidents
        WHERE CONVERT(varchar(11), AssignedSecure, 111) BETWEEN CONVERT(varchar(11), '{dateString}', 111)
            AND CONVERT(varchar(11), DATEADD(DAY, 7, '{dateString}'), 111)";

            var dt3 = dl.LoadData(incidentQry);
            List<PlanningTask2> tasks = new List<PlanningTask2>();

            foreach (DataRow row in dt3.Rows)
            {

                DateTime.TryParse(row["SECUREDATE"].ToString(), out DateTime startUtc);
                DateTime.TryParse(row["FIXEDDATE"].ToString(), out DateTime endUtc);



                PlanningTask2 task = new PlanningTask2
                {
                    IncidentNo = Convert.ToInt32(row["Incidentno"]),
                    UserId = Convert.ToInt32(row["UserId"]),
                    AssignedSecureDate = row["AssignedSecureDate"].ToString(),
                    TimeAssignSecure = row["TimeAssignSecure"].ToString(),
                    TimeAssignFixed = row["TimeAssignFixed"].ToString(),
                    WorkDetail= row["WorkDetail"].ToString() ,
                    StartDateTimeUtc = startUtc,
                    EndDateTimeUtc = endUtc
                };
                tasks.Add(task);
            }
            return Json(tasks);
        }


        public IActionResult Updateplanningtask(string startTime, string endTime, string incidentno, string startdate, string enddate ,int userid , DateTime startDateTimeUtc, DateTime endDateTimeUtc)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Convert incidentno to integer since your model suggests it's an integer.
                    int incidentNumber = int.Parse(incidentno);

                    var incident = _context.TblIncidents.SingleOrDefault(i => i.IncidentNo == incidentNumber);

                    if (incident != null)
                    {
                        // Parse and format date and time strings
                        DateTime startDateTime = DateTime.ParseExact(startdate + " " + startTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                        DateTime endDateTime = DateTime.ParseExact(enddate + " " + endTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);

                        // Assign the DateTime objects to the properties
                        incident.AssignedSecure = startDateTimeUtc;
                        incident.AssignedFixed = endDateTimeUtc;
                        incident.MechanicId = userid;


                        _context.SaveChanges();
                        transaction.Commit();
                        return Json(true);
                    }
                    else
                    {
                        transaction.Rollback();
                        return Json(new { success = false, message = "Incident not found." });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = ex.Message });
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

    

    public IActionResult Planning(DateTime? date)
        { 

              var qryform = _context.Tblownforms.Select(f => new Tblownform { Formid = f.Formid, Formname = f.Formname }).ToList();

              ViewData["WorkOrder"] = qryform;
              DataLogic dl = new DataLogic(_configuration);
            DateTime inputDate = date ?? DateTime.Now;

            
            string dateString = inputDate.ToString("yyyy-MM-dd");
     
            string planningSlotQry = $@"
        DECLARE @InputDate DATE ='{dateString}';
        WITH DateSequence AS (
            SELECT @InputDate AS DateValue, 1 AS DayCounter
            UNION ALL
            SELECT DATEADD(DAY, 1, DateValue), DayCounter + 1
            FROM DateSequence
            WHERE DayCounter < 7
        )
        SELECT FORMAT(DateValue, 'dd-MM-yyyy') AS Date, S.TimeSlot 
        FROM DateSequence
        CROSS JOIN TBLSLOT S
        ORDER BY DateValue, S.TimeSlot";

            var dt1 = dl.LoadData(planningSlotQry);

            List<PlanningSlot> planningSlots = new List<PlanningSlot>();

            foreach (DataRow row in dt1.Rows)
            {
                PlanningSlot slot = new PlanningSlot
                {
                    Date = row["Date"].ToString(),
                    TimeSlot = (TimeSpan)row["TimeSlot"]
                };
                planningSlots.Add(slot);
            }

            // Query to get Users
            string userQry = @"
        SELECT * FROM TblUsers
        WHERE UserTypeId = 3
        ORDER BY UserId";

            var dt2 = dl.LoadData(userQry);

            List<TblUser> users = new List<TblUser>();

            foreach (DataRow row in dt2.Rows)
            {
                TblUser user = new TblUser
                {
                    UserId = Convert.ToInt32(row["UserId"]),
                    UserName = row["UserName"].ToString(),
                    Password = row["Password"].ToString(),
                    UserTypeId = Convert.ToInt32(row["UserTypeId"]),
                    SkillId = Convert.ToInt32(row["SkillId"]),
                    FcmToken = row["FcmToken"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    SecondName = row["SecondName"].ToString(),
                    Gender = row["Gender"].ToString(),
                    Email = row["Email"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Address = row["Address"].ToString(),
                    EmergencyNo = row["EmergencyNo"].ToString(),
                    AccessToken = row["AccessToken"].ToString(),
                    Otp = row["Otp"].ToString(),
                    AppAccess = row["AppAccess"] as bool?,
                    DashboardAccess = row["DashboardAccess"] as bool?,
                    ReceiveIncAlert = row["ReceiveIncAlert"] as bool?,
                    ImgPath = row["ImgPath"].ToString(),
                    AllowCall = row["AllowCall"] as bool?,
                    AllowSms = row["AllowSms"] as bool?,
                    AllowWhatsapp = row["AllowWhatsapp"] as bool?,
                    AllowEmail = row["AllowEmail"] as bool?,
                    InTime = row["InTime"] as DateTime?,
                    OutTime = row["OutTime"] as DateTime?
                };
                users.Add(user);
            }

            string dateString1 = inputDate.ToString("yyyy/MM/dd");
            // Query to get Incidents with TimeAssignSecure and TimeAssignFixed
            string incidentQry = $@"
        SELECT Incidentno, 
       MechanicId AS UserId,
       AssignedSecure,
       AssignedFixed,
       FORMAT(AssignedSecure, 'HH:mm') AS TimeAssignSecure,
       FORMAT(AssignedFixed, 'HH:mm') AS TimeAssignFixed
FROM TblIncidents
WHERE     CONVERT(varchar(11),AssignedSecure,111)     BETWEEN      CONVERT(varchar(11), '{dateString}',111)   AND  CONVERT(varchar(11), DATEADD(DAY, 7, '{dateString}'),111)    ";


            var dt3 = dl.LoadData(incidentQry);

            List<PlanningTask> tasks = new List<PlanningTask>();

            foreach (DataRow row in dt3.Rows)
            {
                PlanningTask task = new PlanningTask
                {
                    IncidentNo = Convert.ToInt32(row["Incidentno"]),
                    UserId = Convert.ToInt32(row["UserId"]),
                    AssignedSecure = Convert.ToDateTime(row["AssignedSecure"]),
                    AssignedFixed = Convert.ToDateTime(row["AssignedFixed"])
                };
                tasks.Add(task);
            }

            // Create a view model and pass it to the view
            var viewModel = new PlanningViewModel
            {
                PlanningSlots = planningSlots,
                Users = users,
                Tasks = tasks // Add tasks to the view model
            };
            ViewBag.SelectedDate = inputDate;
            return View(viewModel);
        }














        public IActionResult WorkOrderPartiallyLoad()
        {
            var qryform = _context.Tblownforms.Select(f => new Tblownform {Formid = f.Formid,Formname = f.Formname}).ToList();



           
            // Return the view as a partial view without layout


            return PartialView("~/Views/PartialViews/WorkOrder/WorkOrder.cshtml", qryform);
        }

        
     
   





    }
}