using CityTechWEBAPI;
using MediaOutDoor.Models;
using MediaOutDoor.Services;
using MediaOutDoor.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Transactions;

namespace MediaOutDoor.Controllers
{
    public class OrderController : Controller
    {

        private readonly MediaOutdoorContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IEmail _email;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webRoot;


        public OrderController(MediaOutdoorContext dbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IEmail email, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webRoot)
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



        #region Private/Commercial



        public IActionResult Private()
        {
            List<TblContentCat> category = _dbContext.TblContentCats
                .AsNoTracking().ToList();
            return View(category);
        }

        [ActionName("PrivateDetail")]
        public IActionResult PrivateDetail(int id)
        {
            List<TblContentDetail> content = _dbContext.TblContentDetails
                .Where(x => x.CatId == id)
                .AsNoTracking().ToList();

            ViewBag.b2cRate = _dbContext.TblSettings.Select(s => s.RateB2c).FirstOrDefault();

            return View(content);
        }


        public IActionResult Commercial()
        {
            ViewBag.b2bRate = _dbContext.TblSettings.Select(s => s.RateB2b).FirstOrDefault();
            return View();
        }

        public JsonResult GetCpmEntry()
        {
            return Json(_dbContext.TblCpms.AsNoTracking().OrderBy(o => o.Id).ToList());
        }

        #endregion


        #region Order Completed / My Orders

        [UserAuthentication]
        public IActionResult MyOrders()
        {
            return View();
        }

        public string GetOrdersCompleted()
        {
            var csid = new SessionData(_httpContextAccessor).GetData();

            DataLogic dl = new DataLogic(_configuration);

            String qry = @"select distinct 1 Sno,OrderNo,DATE,STATUS,ORDERTYPE,CUSTOMERID
                           INTO #O
                           from Tblorders
                           select top 1 1 Sno, Playdate,SlotFrom,Slotto
                           INTO #OS
                           from TblOrderSchedule order by Playdate asc
                           select CONVERT(BIGINT,dbo.ExtractNumericValues(OrderNo))
                           OrderSort,orderNo,date,status,ordertype , playDate,slotFrom,slotTo,customerId
                           from #O INNER JOIN #OS ON #O.SNO=#OS.SNO where status = 'New'
                           and customerid = '" + csid.UserId + "' order by OrderSort";

            return JsonConvert.SerializeObject(dl.LoadData(qry));
        }

        public string ViewOrdersCompleted(string id)
        {
            var csid = new SessionData(_httpContextAccessor).GetData();

            DataLogic dl = new DataLogic(_configuration);

            String qry = @"SELECT O.ORDERID AS id, O.ORDERNO AS orderNo, O.DATE AS date,
                           C.CUSTOMERID AS customerId, C.FIRSTNAME + ' ' + C.SECONDNAME AS customerName,
                           C.EMAIL AS customerEmail, ST.STATIONID AS stationId,
                           ST.STATIONNAME AS stationName, SC.SCREENSIZE as size, SC.SCREENID AS screenId,
                           SC.SCREENNAME AS screenName, O.RATE AS rate, O.AMOUNT AS amount,
                           O.STATUS AS status,
                           O.ORDERIMAGE AS image, ISNULL(O.PAYMENTMETHOD,'') AS paymentMethod, O.ORDERTYPE AS ordertype
                           
                           FROM TBLORDERS O
                           INNER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = O.CUSTOMERID
                           INNER JOIN TBLSCREENS SC ON SC.SCREENID = O.SCREENID
                           INNER JOIN TBLSTATIONS ST ON ST.STATIONID = O.STATIONID
                           
                           WHERE O.STATUS = 'New' AND O.CUSTOMERID = '" + csid.UserId + "' AND O.ORDERNO = '" + id + "' ORDER BY O.ORDERID;";

            String qry2 = @"SELECT DISTINCT OS.SLOTFROM AS slotFrom, OS.SLOTTO AS slotTo, OS.PLAYDATE AS playDate
                           FROM TBLORDERS O
						   INNER JOIN TBLORDERSCHEDULE OS ON OS.VISITORID = O.VISITORID
                           
                           WHERE O.STATUS = 'new' AND O.CUSTOMERID = '" + csid.UserId + "' AND O.ORDERNO = '" + id + "' ORDER BY OS.PLAYDATE;";


            var dt1 = dl.LoadData(qry);
            var dt2 = dl.LoadData(qry2);

            var data = new
            {
                detail = dt1,
                slots = dt2

            };

            return JsonConvert.SerializeObject(data);
        }

        #endregion


        public IActionResult EditYourOrder()
        {
            return View();
        }


        public string GetCartbyCustomer()
        {

            DataLogic dl = new DataLogic(_configuration);

            var csid = new SessionData(_httpContextAccessor).GetData();

            string qry = "Select c.ScreenId,c.VisitorId,sc.ScreenName, c.orderimage, c.text1, c.text2, c.text3, c.Text1Color, c.Text2Color, c.Text3Color,sc.ScreenSize, sc.Rate, st.StationId, st.StationName,\r\nc.text1font, c.text2font, c.text3font, c.Text1TopPosition, c.Text1LeftPosition, c.Text2TopPosition, c.Text2LeftPosition,c.Text3TopPosition, c.Text1Size,c.Text2Size,c.Text3Size, c.Text3LeftPosition  from TblOrders c\r\nLeft Join TblScreens sc On sc.screenid = c.screenId\r\nLeft Join tblStations st On st.stationid = sc.stationid where c.customerid = '" + csid.UserId + "'";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }



        public IActionResult BudgetRate(string VisitorId, float Rate, string OrderType)
        {
            var OrdrCartList = _dbContext.TblOrders.Where(order => order.VisitorId == VisitorId).ToList();

            if (OrdrCartList.Count > 0)
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        foreach (var order in OrdrCartList)
                        {
                            
                            order.Rate = Rate;
                            order.OrderType = OrderType;

                        }

                        _dbContext.SaveChanges();

                        scope.Complete();

                        return Json("Rates updated for all orders in the cart");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction on exception
                        scope.Dispose(); // This will perform a rollback
                        return Json("An error occurred while updating rates: " + ex.Message);
                    }
                }
            }

            // Handle the case where no orders were found for the given VisitorId.
            return Json("No orders found for the specified VisitorId.");
        }



    }
}
