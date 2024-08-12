using CityTech.Models;
using CityTech.Sevices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace CityTech.Controllers
{
    public class ReportController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly CityTechContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportController(CityTechContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }


        #region Log
        [AllowPage]
        public IActionResult ActivityLogs()
        {
            return View();
        }

        public IActionResult GetUsers()
        {
            return Json(_context.TblUsers.Select(s=> new {id = s.UserId, name = s.UserName}));
        }


        public string ActivityLogsList(string fromDate, string toDate, string userid, string tag)
        {
            if(userid == null){userid = "%";}

            DataLogic dl = new DataLogic(_configuration);
            String qry1 = @"SELECT ISNULL( U.USERNAME, '') AS USERNAME, ISNULL( UT.USERTYPE,'') AS USERTYPE, ISNULL( L.LOGDATE, '') AS DATE,
                            ISNULL( L.ACTIVITY,'') AS ACTIVITY, ISNULL(CAST(L.INCIDENTNO AS VARCHAR(10)), '') AS INCIDENTNO, ISNULL( T.INCIDENTNAME,'') AS INCIDENTNAME,
                            ISNULL(L.LATITUDE, '') AS LATITUDE, ISNULL(L.LONGITUDE, '') AS LONGITUDE
                            FROM TBLLOG L
                            LEFT JOIN TBLINCIDENTS I ON I.INCIDENTNO = L.INCIDENTNO
                            LEFT JOIN TBLINCIDENTTYPES T ON T.INCIDENTTYPEID = I.INCIDENTTYPEID
                            LEFT JOIN TBLUSERS U ON L.USERID = U.USERID
                            LEFT JOIN TBLUSERTYPES UT ON U.USERTYPEID = UT.USERTYPEID
                            WHERE CONVERT(VARCHAR(11),L.LOGDATE,111) BETWEEN '" + fromDate + "' AND '" + toDate + "' AND ISNULL(U.USERID ,'') LIKE '" + userid + "' AND L.TAG LIKE '"+ tag +"'  ORDER BY L.LOGDATE DESC";

            var dt1 = dl.LoadData(qry1);

            return JsonConvert.SerializeObject(dt1);
        }


        #endregion

    }
}
