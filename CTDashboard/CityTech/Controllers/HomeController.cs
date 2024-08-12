using CityTech.Models;
using CityTech.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
//using ImageMagick;
namespace CityTech.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CityTechContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, CityTechContext context, IHttpContextAccessor httpContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContext;
            _hostingEnvironment = webHostEnvironment;
            _configuration = configuration;
        }



        //[HttpPost]
        //    public IActionResult UploadImage(IFormFile imageFile)
        //    {
        //        if (imageFile != null && imageFile.Length > 0)
        //        {
        //            using (var image = new MagickImage(imageFile.OpenReadStream()))
        //            {

        //                image.Quality = (int)(image.Quality * 0.5);


        //                image.Format = MagickFormat.WebP;


        //                // Save the converted image
        //                var outputPath = "path_to_save_webp_image.webp";
        //                image.Write(outputPath);
        //            }
        //        }

        //        return RedirectToAction("Index"); // Redirect to a relevant page
        //    }

        [UserAuthentication]
        [AllowPage]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chart()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult DashboardNew()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> Login(string userName, string one, string two, string three, string four, string five, string six)
        {
            var data = new[]
            {
            new object[] { "Category 1", 20 },
            new object[] { "Category 2", 50 },
            new object[] { "Category 3", 30 },
            new object[] { "Category 4", 40 },
            };

            var password = one + two + three + four + five + six;

            ViewBag.ChartData = data;

            if (userName == null)
            {
                ViewBag.Error = "Please enter user name";
                return View();
            }
            else if (password == null)
            {
                ViewBag.Error = "Please enter password";
                return View();
            }

            //if (password == null && userName == null)
            //{
            //    return View();
            //}
            //else if (userName == null)
            //{
            //    ViewBag.Error = "Please enter user name";
            //    return View();
            //}
            //else if (password == null)
            //{
            //    ViewBag.Error = "Please enter password";
            //    return View();
            //}

            // Mechanic User Restrition For Dashboard Login Start

            //var userRestrict = (from U in _context.TblUsers
            //                    join S in _context.TblUserTypes on U.UserTypeId equals S.UserTypeId
            //                    where (
            //                    (U.UserName.ToLower().Equals(userName.ToLower())) &&
            //                    (U.Password.Equals(password)) && S.UserType.Equals("Mechanic"))
            //                    select new
            //                    {
            //                        user = S.UserType
            //                    }).FirstOrDefault();

            //if (userRestrict != null)
            //{
            //    if (userRestrict.user.Equals("Mechanic"))
            //    {
            //        ViewBag.Error = "You are not authorised";
            //        return View();
            //    }
            //}

            // Mechanic User Restrition For Dashboard Login End

            var dbuser = (from U in _context.TblUsers
                          join T in _context.TblUserTypes on U.UserTypeId equals T.UserTypeId
                          where ((U.UserName.ToLower().Equals(userName.ToLower())) &&
                          (U.Password.Equals(password) &&
                          T.UserType != "Mechanic") && U.DashboardAccess == true)
                          select new
                          {
                              id = U.UserId,
                              name = U.UserName,
                              password = U.Password,
                              type = T.UserType,
                              typeId = T.UserTypeId,
                              FcmToken = U.FcmToken ?? ""
                          }).FirstOrDefault();

            if (dbuser == null)
            {
                ViewBag.Error = "User Name or Password is Incorrect";
                return View();
            }

            HttpContext.Session.SetString("UserId", Convert.ToString(dbuser.id));
            HttpContext.Session.SetString("UserName", Convert.ToString(dbuser.name));
            HttpContext.Session.SetString("Password", Convert.ToString(dbuser.password));
            HttpContext.Session.SetString("UserType", Convert.ToString(dbuser.type));
            HttpContext.Session.SetString("UserTypeId", Convert.ToString(dbuser.typeId));
            //HttpContext.Session.SetString("UserSkill", Convert.ToString(dbuser.skill));
            //HttpContext.Session.SetString("UserSkillId", Convert.ToString(dbuser.skillId));
            string fcmToken = dbuser.FcmToken != null ? Convert.ToString(dbuser.FcmToken) : "";
            HttpContext.Session.SetString("FcmToken", fcmToken);

            return RedirectToAction("DashBoard");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("Password");
            HttpContext.Session.Remove("UserType");
            HttpContext.Session.Remove("UserTypeId");
            HttpContext.Session.Remove("UserSkill");
            HttpContext.Session.Remove("UserSkillId");

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return Json("AccessDenied");
        }



        public string GetDashboard()
        {
            DataLogic dl = new DataLogic(_configuration);
            var SessionData = new SessionData(_httpContextAccessor).GetData();
            String Qry = @"select Id, Tabid, TabContent from tbldashboard where  tabid='" + SessionData.UserId + "'";
            var dt1 = dl.LoadData(Qry);

            return JsonConvert.SerializeObject(dt1);

        }

        public IActionResult GetPartial(string status, string fromDate, string toDate, string location, string objects)
        {
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;
            ViewData["Location"] = location;
            ViewData["Objects"] = objects;
            return PartialView("/Views/PartialViews/Dashboard/" + status + ".cshtml");
        }
        public string IncidentCount(string fromDate, string toDate, string location, string objects)
        {
            DataLogic dl = new DataLogic(_configuration);
            String Qry1 = @"select Count(*)  COUNT from TBLINCIDENTS where isnull(WorkEnd, 0) = 0 and isnull(IsScheduled,0)= 1   AND ISNULL(LOCID,0) LIKE '" + location + "' AND ISNULL(OBJECTID,0) LIKE '" + objects + "' ";
            String Qry2 = @"select Count(*)  COUNT from TBLINCIDENTS where isnull(IsScheduled, 0) = 0 AND ISNULL(LOCID,0) LIKE '" + location + "' AND ISNULL(OBJECTID,0) LIKE '" + objects + "' ";
            var dt1 = dl.LoadData(Qry1);
            var dt2 = dl.LoadData(Qry2);
            var IncidentCount = new
            {
                Open = dt1,
                Unassign = dt2
            };
            return JsonConvert.SerializeObject(IncidentCount);
        }

        public string SensorCount(string fromDate, string toDate, string location, string objects)
        {
            DataLogic dl = new DataLogic(_configuration);
            String Qry1 = @"select       ITYPES.INCIDENTNAME DETAIL , Count(*) COUNT  from dbo.TblIncidents INC
INNER JOIN TBLINCIDENTTYPES ITYPES ON ITYPES.INCIDENTTYPEID = INC.INCIDENTTYPEID
where ITYPES.INCIDENTNAME IN ( 'DOOR SENSOR','GLASS SENSOR')   AND ISNULL(LOCID,0) LIKE '" + location + "' AND ISNULL(OBJECTID,0) LIKE '" + objects + "'  AND  CONVERT(varchar(11), IncidentDate, 111) BETWEEN '" + fromDate + "' AND '" + toDate + "'    group by  ITYPES.INCIDENTNAME order by DETAIL desc  ";
            var dt1 = dl.LoadData(Qry1);
            var SensorCount = new
            {
                SensorCount = dt1,
            };
            return JsonConvert.SerializeObject(SensorCount);
        }

        public string RecentLocations(string fromDate, string toDate, string location, string objects)
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry = @"select DISTINCT     S.Station  +  ' - ' + B.OBJECTNAME LOCATION  from dbo.TblIncidents INC
INNER JOIN TBLOBJECTS B ON B.OBJECTID= INC.OBJECTID
INNER JOIN TblCustomers C ON C.CUSTOMERID= B.CUSTOMERID
INNER JOIN  TblObjectLocations L ON  B.LOCID=L.LOCID
INNER JOIN  TblStation S ON S.ID= L.STATIONID
WHERE  ISNULL(INC.LOCID,0) LIKE '" + location + "' AND ISNULL(INC.OBJECTID,0) LIKE '" + objects + "'  AND  CONVERT(varchar(11), IncidentDate, 111) BETWEEN '" + fromDate + "' AND '" + toDate + "'   AND INC.INCIDENTNO IN ( SELECT TOP 5  INCIDENTNO FROM    TblIncidents   where ISNULL(LOCID,0) LIKE '" + location + "' AND ISNULL(OBJECTID,0) LIKE '" + objects + "' AND  CONVERT(varchar(11), IncidentDate, 111) BETWEEN '" + fromDate + "' AND '" + toDate + "'  order by incidentno desc)   ";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }


        public string MaintenanceHistory(string fromDate, string toDate, string location, string objects)
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry = @"select DISTINCT      U.USERNAME DETAIL ,  LTRIM(STR(DAY(WORKEND)))+'-'+LTRIM(STR(MONTH(WORKEND))) DATEDETAIL    from dbo.TblIncidents INC
INNER JOIN TBLUSERS U ON U.USERID=MECHANICID
WHERE ISNULL(WORKDONE,0)>0 AND ISNULL(INC.LOCID,0) LIKE '" + location + "' AND ISNULL(INC.OBJECTID,0) LIKE '" + objects + "'  AND  CONVERT(varchar(11), IncidentDate, 111) BETWEEN '" + fromDate + "' AND '" + toDate + "' --ORDER BY WORKEND DESC";
            var dt = dl.LoadData(qry);
            return JsonConvert.SerializeObject(dt);
        }

        public string FaultHistory(string fromDate, string toDate, string location, string objects)
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry = @"select DISTINCT      ITYPES.INCIDENTNAME DETAIL ,  LTRIM(STR(DAY(WORKEND)))+'-'+LTRIM(STR(MONTH(WORKEND))) DATEDETAIL    from dbo.TblIncidents INC
INNER JOIN TBLINCIDENTTYPES ITYPES ON ITYPES.INCIDENTTYPEID = INC.INCIDENTTYPEID 
WHERE ISNULL(WORKDONE,0)>0 AND ISNULL(INC.LOCID,0) LIKE '" + location + "' AND ISNULL(INC.OBJECTID,0) LIKE '" + objects + "' AND  CONVERT(varchar(11), IncidentDate, 111) BETWEEN '" + fromDate + "' AND '" + toDate + "' --ORDER BY WORKEND DESC";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }

        public string OpenVouchers(string fromDate, string toDate, string location, string objects)
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry = @"select IncidentNo INCNO ,   upper(  S.Station  +  ' - ' + B.OBJECTNAME + ' ('+INCT.IncidentName +')' )  DETAIL     from dbo.TblIncidents INC
INNER JOIN  TblIncidentTypes INCT ON INCT.INCIDENTTYPEID =  INC.INCIDENTTYPEID
INNER JOIN TBLOBJECTS B ON B.OBJECTID= INC.OBJECTID
INNER JOIN TblCustomers C ON C.CUSTOMERID= B.CUSTOMERID
INNER JOIN  TblObjectLocations L ON  B.LOCID=L.LOCID
INNER JOIN  TblStation S ON S.ID= L.STATIONID
WHERE  ISNULL(WORKEND,0)=0 AND ISNULL(INC.LOCID,0) LIKE '" + location + "' AND ISNULL(INC.OBJECTID,0) LIKE '" + objects + "'  AND  CONVERT(varchar(11), INC.IncidentDate, 111) BETWEEN '" + fromDate + "' AND '" + toDate + "'   ORDER BY INCNO DESC";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }
        public string ObjectTemperature(string fromDate, string toDate, string location, string objects)
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry = @"select ltrim(str(day( date))) +'-'+ltrim(str(MONTH(date))) Xasix , isnull(value,0) Datavalue  from tblobjectsactivity objac 
inner join TblObjects obj on obj.ObjectId=objac.Objectid
Where tag='TEMPERATURE' AND  ISNULL(OBJ.LOCID,0) LIKE '" + location + "'  AND  ISNULL(objac.Objectid,0) LIKE '" + objects + "'    AND  CONVERT(varchar(11), date, 111)  BETWEEN '" + fromDate + "' AND '" + toDate + "'   order by date ";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }
        public string Brightness(string fromDate, string toDate, string location, string objects)
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry = @"select FORMAT(date, 'HH:mm') AS  Xasix , isnull(value,0) Datavalue  from tblobjectsactivity objac 
inner join TblObjects obj on obj.ObjectId=objac.Objectid
Where tag='BRIGHTNESS' AND  ISNULL(OBJ.LOCID,0) LIKE '" + location + "'  AND  ISNULL(objac.Objectid,0) LIKE '" + objects + "'   AND  CONVERT(varchar(11), date, 111) = '" + toDate + "'     order by date ";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }
        public string Assets(string fromDate, string toDate, string location, string objects)
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry = @"select FORMAT(date, 'HH:mm') AS  Xasix , isnull(value,0) Datavalue  from tblobjectsactivity objac
inner join TblObjects obj on obj.ObjectId=objac.Objectid
Where tag='ASSETS' AND  ISNULL(OBJ.LOCID,0) LIKE '" + location + "'  AND  ISNULL(objac.Objectid,0) LIKE '" + objects + "'   AND  CONVERT(varchar(11), date, 111) = '" + toDate + "'   order by date ";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }
        public string SLAAverage(string fromDate, string toDate, string location, string objects)
        {
            DataLogic dl = new DataLogic(_configuration);
            String qry = @"IF OBJECT_ID('tempdb..#temptransSLA') IS NOT NULL DROP TABLE #temptransSLA
seleCT  INCIDENTNO,   DATEDIFF(MINUTE, IncidentDate, ScheduleDate)    Responsetime ,  ISNULL((case when isnull(DATEDIFF(MINUTE, IncidentDate, ScheduleDate),0)<=(isnull(INCTYPE.SLAResponse,0)*60 ) THEN 100 ELSE 0 END),0)  SLARESPONSE
INTO #temptransSLA
from TblIncidents INC
INNER JOIN TblIncidentTypes INCTYPE ON INCTYPE.IncidentTypeId = INC.IncidentTypeId
where isnull(IsScheduled,0)=1  and isnull(workdone,0)=1   AND  ISNULL(INC.LOCID,0) LIKE '" + location + "' AND ISNULL(INC.OBJECTID,0) LIKE '" + objects + "' SELECT  COUNT(INCIDENTNO) COUNT ,    SUM(Responsetime)/COUNT(INCIDENTNO)AVGTIME , SUM(SLARESPONSE)/COUNT(INCIDENTNO)  SLAWIN  FROM  #temptransSLA ";

            string qry1 = @"IF OBJECT_ID('tempdb..#temptransSLA1') IS NOT NULL DROP TABLE #temptransSLA1 " +
 " SELECT Isnull( case when  ISNULL(SUM(DATEDIFF(MINUTE, IncidentDate, ScheduleDate)), 0) > 0 and  count(*) > 0 then  ISNULL(SUM(DATEDIFF(MINUTE, IncidentDate, ScheduleDate)), 0) / count(*)  else 0 end,0) LastWeek  , 0 ThisWeek  " +
 " into #temptransSLA1 " +
 " FROM TblIncidents " +
 " WHERE CONVERT(varchar(11),IncidentDate,111)  >= CONVERT(varchar(11), DATEADD(week, -2, '" + toDate + "'), 111)  AND CONVERT(varchar(11),IncidentDate,111) < CONVERT(varchar(11), DATEADD(week, -1, '" + toDate + "'), 111)   AND isnull(IsScheduled,0)= 1 " +
 " union all " +
 " SELECT 0 LastWeek ,Isnull( case when  ISNULL(SUM(DATEDIFF(MINUTE, IncidentDate, ScheduleDate)), 0) > 0 and  count(*) > 0 then  ISNULL(SUM(DATEDIFF(MINUTE, IncidentDate, ScheduleDate)), 0) / count(*)  else 0 end,0) ThisWeek FROM TblIncidents " +
 " WHERE CONVERT(varchar(11), IncidentDate, 111) >= CONVERT(varchar(11), DATEADD(week, -1, '" + toDate + "'), 111)   AND CONVERT(varchar(11),IncidentDate,111) < = CONVERT(varchar(11), '" + toDate + "', 111)     AND isnull(IsScheduled,0)= 1 " +
 " select  sum(LastWeek) LastWeek  ,   sum(LastWeek)  - Sum(ThisWeek)  ThisWeek, (case when isnull(sum(LastWeek)  - Sum(ThisWeek)  ,0)<0 then isnull(sum(LastWeek)  - Sum(ThisWeek)  ,0) *-1 else isnull(sum(LastWeek)  - Sum(ThisWeek)  ,0) end ) ThisWeek1  from #temptransSLA1";

            var dt = dl.LoadData(qry);
            var dt1 = dl.LoadData(qry1);

            var SLAAverage = new
            {
                SLA1 = dt,
                SLA2 = dt1
            };

            return JsonConvert.SerializeObject(SLAAverage);


        }
        public IActionResult SaveDashboard(string Id, string Tabid, string TabContent)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var SessionData = new SessionData(_httpContextAccessor).GetData();
                    var DashBoard = _context.Tbldashboards.Where(x => x.Tabid.Equals(SessionData.UserId.ToString())).FirstOrDefault();
                    if (DashBoard == null)
                    {
                        int Newid = 0;
                        int? MaxNewid = _context.Tbldashboards.Max(i => (int?)i.Id);
                        if (MaxNewid.HasValue)
                        {
                            Newid = MaxNewid.Value + 1;
                        }
                        else
                        {

                            Newid = 1;
                        }
                        _context.Tbldashboards.Add(new Tbldashboard
                        {
                            Id = Newid,
                            Tabid = SessionData.UserId.ToString(),
                            TabContent = TabContent,

                        });
                    }
                    else
                    {
                        DashBoard.Tabid = SessionData.UserId.ToString();
                        DashBoard.TabContent = TabContent;
                        _context.Tbldashboards.Update(DashBoard);
                    }
                    _context.SaveChanges();
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