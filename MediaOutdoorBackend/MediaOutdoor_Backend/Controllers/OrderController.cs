using MediaOutdoor_Backend.Models;
using MediaOutdoor_Backend.Services;
using MediaOutdoor_Backend.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Transactions;

namespace MediaOutdoor_Backend.Controllers
{
    public class OrderController : Controller
    {
        private readonly MediaOutdoorContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webRoot;
        private readonly IEmail _email;

        public OrderController(MediaOutdoorContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webRoot, IEmail email)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webRoot = webRoot;
            _email = email;
        }

        private bool IsPrimaryKeyViolation(DbUpdateException ex)
        {
            // Check if the exception indicates a primary key violation
            // This can vary depending on the database provider you're using
            // You may need to adapt this based on your database provider's error codes or messages
            return ex.InnerException is SqlException sqlException && sqlException.Number == 2627;
        }

        #region ViewModels


        public class SaveDesignRequest
        {
            public List<ScreenData> ScreenDataArray { get; set; }
        }

        public class ScreenData
        {
            public int ScreenId { get; set; }
            public string LeftValue1 { get; set; }
            public string TopValue1 { get; set; }
            public string LeftValue2 { get; set; }
            public string TopValue2 { get; set; }
            public string LeftValue3 { get; set; }
            public string TopValue3 { get; set; }

        }

        #endregion

        #region Order Completed

        public IActionResult OrderCompleted()
        {
            return View();
        }

        public string GetOrdersCompleted(string OrderDate, string SlotDate)
        {
            DataLogic dl = new DataLogic(_configuration);

            string dates = "";

            if (SlotDate != null)
            {
                dates = " and PlayDate = '" + SlotDate + "' ";
            }

            if (OrderDate != null)
            {
                dates = " and date = '" + OrderDate + "' ";
            }

            if (SlotDate != null && OrderDate != null)
            {
                dates = " and date = '" + OrderDate + "' and PlayDate = '" + SlotDate + "' ";
            }

            if (SlotDate == null && OrderDate == null)
            {
                dates = "";
            }

            String qry = @"select distinct 1 Sno,OrderNo,DATE,STATUS,ORDERTYPE
                           INTO #O
                           from Tblorders
                           select top 1 1 Sno, Playdate,SlotFrom,Slotto
                           INTO #OS
                           from TblOrderSchedule order by Playdate asc
                           select CONVERT(BIGINT,dbo.ExtractNumericValues(OrderNo))
                           OrderSort,orderNo,date,status,ordertype , playDate,slotFrom,slotTo
                           from #O INNER JOIN #OS ON #O.SNO=#OS.SNO where status = 'completed'
                           " + dates + "order by OrderSort";

            return JsonConvert.SerializeObject(dl.LoadData(qry));
        }

        public string ViewOrdersCompleted(string id)
        {
            DataLogic dl = new DataLogic(_configuration);

            String qry = @"SELECT O.ORDERID AS id, O.ORDERNO AS orderNo, O.VISITORID AS visitorid, O.DATE AS date,
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
                           
                           WHERE O.STATUS = 'completed' AND O.ORDERNO = '" + id + "' ORDER BY O.ORDERID;";

            String qry2 = @"SELECT DISTINCT OS.SLOTFROM AS slotFrom, OS.SLOTTO AS slotTo, OS.PLAYDATE AS playDate
                           FROM TBLORDERS O
						   INNER JOIN TBLORDERSCHEDULE OS ON OS.VISITORID = O.VISITORID
                           
                           WHERE O.STATUS = 'completed' AND O.ORDERNO = '" + id + "' ORDER BY OS.PLAYDATE;";


            var dt1 = dl.LoadData(qry);
            var dt2 = dl.LoadData(qry2);

            var data = new
            {
                detail = dt1,
                slots = dt2

            };

            return JsonConvert.SerializeObject(data);
        }


        public IActionResult SaveOrderCompleted(int id, string remarks, string emailSent)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var orderNo = "";
                try
                {
                    if (id != 0 && remarks != null)
                    {
                        var order = _context.TblOrders.Where(x => x.OrderId == id).FirstOrDefault();
                        if (order != null)
                        {
                            orderNo = order.OrderNo;

                            order.Status = "processing";
                            order.Remarks = remarks;
                            _context.TblOrders.Update(order);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        return Json(false);
                    }
                    transaction.Commit();

                    if(emailSent != null)
                    {
                        _email.SendEmail(emailSent, "Order Processing", "Your order no " + orderNo + " is in processing. Because " + remarks);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }

            return Json(true);
        }


        #endregion

        #region Order Processing

        public IActionResult OrderProcessing()
        {
            return View();
        }

        public string GetOrdersProcessing(string OrderDate, string SlotDate)
        {
            DataLogic dl = new DataLogic(_configuration);

            string dates = "";

            if (SlotDate != null)
            {
                dates = " and PlayDate = '" + SlotDate + "' ";
            }

            if (OrderDate != null)
            {
                dates = " and date = '" + OrderDate + "' ";
            }

            if (SlotDate != null && OrderDate != null)
            {
                dates = " and date = '" + OrderDate + "' and PlayDate = '" + SlotDate + "' ";
            }

            if (SlotDate == null && OrderDate == null)
            {
                dates = "";
            }

            String qry = @"select distinct 1 Sno,OrderNo,DATE,STATUS,ORDERTYPE
                           INTO #O
                           from Tblorders
                           select top 1 1 Sno, Playdate,SlotFrom,Slotto
                           INTO #OS
                           from TblOrderSchedule order by Playdate asc
                           select CONVERT(BIGINT,dbo.ExtractNumericValues(OrderNo))
                           OrderSort,orderNo,date,status,ordertype , playDate,slotFrom,slotTo
                           from #O INNER JOIN #OS ON #O.SNO=#OS.SNO where status = 'approved'
                           " + dates + "order by OrderSort";

            return JsonConvert.SerializeObject(dl.LoadData(qry));
        }

        public string ViewOrdersProcessing(string id)
        {
            DataLogic dl = new DataLogic(_configuration);

            String qry = @"SELECT O.ORDERID AS id, O.ORDERNO AS orderNo, O.VISITORID AS visitorid, O.DATE AS date,
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
                           
                           WHERE O.STATUS = 'approved' AND O.ORDERNO = '" + id + "' ORDER BY O.ORDERID;";

            String qry2 = @"SELECT DISTINCT OS.SLOTFROM AS slotFrom, OS.SLOTTO AS slotTo, OS.PLAYDATE AS playDate
                           FROM TBLORDERS O
						   INNER JOIN TBLORDERSCHEDULE OS ON OS.VISITORID = O.VISITORID
                           
                           WHERE O.STATUS = 'approved' AND O.ORDERNO = '" + id + "' ORDER BY OS.PLAYDATE;";


            var dt1 = dl.LoadData(qry);
            var dt2 = dl.LoadData(qry2);

            var data = new
            {
                detail = dt1,
                slots = dt2

            };

            return JsonConvert.SerializeObject(data);
        }


        public IActionResult SaveOrderProcessing(string orderNo, string emailSent, string message)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (orderNo != null)
                    {
                        var orders = _context.TblOrders.Where(x => x.OrderNo == orderNo).ToList();

                        if (orders != null && orders.Any())
                        {
                            // Update the status for all matching orders
                            foreach (var order in orders)
                            {
                                order.Status = "Completed";
                            }

                            _context.TblOrders.UpdateRange(orders);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        return Json(false);
                    }
                    transaction.Commit();

                    if (emailSent != null)
                    {
                        _email.SendEmail(emailSent, "Order " + orderNo + " Update", message);
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }

            return Json(true);
        }


        #endregion

        #region Order Disapproved

        public IActionResult OrderDisapproved()
        {
            return View();
        }

        public string GetOrdersDisapproved(string OrderDate, string SlotDate)
        {
            DataLogic dl = new DataLogic(_configuration);

            string dates = "";

            if (SlotDate != null)
            {
                dates = " and PlayDate = '" + SlotDate + "' ";
            }

            if (OrderDate != null)
            {
                dates = " and date = '" + OrderDate + "' ";
            }

            if (SlotDate != null && OrderDate != null)
            {
                dates = " and date = '" + OrderDate + "' and PlayDate = '" + SlotDate + "' ";
            }

            if (SlotDate == null && OrderDate == null)
            {
                dates = "";
            }

            String qry = @"select distinct 1 Sno,OrderNo,DATE,STATUS,ORDERTYPE
                           INTO #O
                           from Tblorders
                           select top 1 1 Sno, Playdate,SlotFrom,Slotto
                           INTO #OS
                           from TblOrderSchedule order by Playdate asc
                           select CONVERT(BIGINT,dbo.ExtractNumericValues(OrderNo))
                           OrderSort,orderNo,date,status,ordertype , playDate,slotFrom,slotTo
                           from #O INNER JOIN #OS ON #O.SNO=#OS.SNO where status = 'disapproved'
                           " + dates + "order by OrderSort";

            return JsonConvert.SerializeObject(dl.LoadData(qry));

        }

        public string ViewOrdersDisapproved(string id)
        {
            DataLogic dl = new DataLogic(_configuration);

            String qry = @"SELECT O.ORDERID AS id, O.ORDERNO AS orderNo, O.VISITORID AS visitorid, O.DATE AS date,
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
                           
                           WHERE O.STATUS = 'disapproved' AND O.ORDERNO = '" + id + "' ORDER BY O.ORDERID;";

            String qry2 = @"SELECT DISTINCT OS.SLOTFROM AS slotFrom, OS.SLOTTO AS slotTo, OS.PLAYDATE AS playDate
                           FROM TBLORDERS O
						   INNER JOIN TBLORDERSCHEDULE OS ON OS.VISITORID = O.VISITORID
                           
                           WHERE O.STATUS = 'disapproved' AND O.ORDERNO = '" + id + "' ORDER BY OS.PLAYDATE;";


            var dt1 = dl.LoadData(qry);
            var dt2 = dl.LoadData(qry2);

            var data = new
            {
                detail = dt1,
                slots = dt2

            };

            return JsonConvert.SerializeObject(data);
        }

        public IActionResult SaveOrderDisapproved(string orderNo, string emailSent, string message)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    if (orderNo != null)
                    {
                        var orders = _context.TblOrders.Where(x => x.OrderNo == orderNo).ToList();

                        if (orders != null && orders.Any())
                        {
                            // Update the status for all matching orders
                            foreach (var order in orders)
                            {
                                order.Status = "Approved";
                            }

                            _context.TblOrders.UpdateRange(orders);
                            _context.SaveChanges();
                        }

                    }
                    else
                    {
                        return Json(false);
                    }
                    transaction.Commit();

                    if (emailSent != null)
                    {
                        _email.SendEmail(emailSent, "Order " + orderNo + " Update", message);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }

            return Json(true);
        }

        #endregion

        #region Order Pending/New

        public IActionResult OrderPending()
        {
            return View();
        }

        public string GetOrdersPending(string OrderDate, string SlotDate)
        {
            DataLogic dl = new DataLogic(_configuration);

            string dates = "";

            if (SlotDate != null)
            {
                dates = " and PlayDate = '" + SlotDate + "' ";
            }

            if (OrderDate != null)
            {
                dates = " and date = '" + OrderDate + "' ";
            }

            if (SlotDate != null && OrderDate != null)
            {
                dates = " and date = '" + OrderDate + "' and PlayDate = '" + SlotDate + "' ";
            }

            if (SlotDate == null && OrderDate == null)
            {
                dates = "";
            }


       



            String qry = @"select distinct 1 Sno,OrderNo,DATE,STATUS,ORDERTYPE , sum( isnull(Rate,0)) Amount  , Isnull( PaymentStatus,0)  PaymentStatus  , isnull( PaymentReference,'') PaymentReference , isnull( TransactionID  ,'') TransactionID
                           
                           INTO #O
                           from Tblorders
                          group by   OrderNo,DATE,STATUS,ORDERTYPE ,   Isnull( PaymentStatus,0)    , isnull( PaymentReference,'')  , isnull( TransactionID  ,'') 
                           select top 1 1 Sno, Playdate,SlotFrom,Slotto 
                           INTO #OS
                           from TblOrderSchedule order by Playdate asc
                           select CONVERT(BIGINT,dbo.ExtractNumericValues(OrderNo))
                           OrderSort,orderNo,date,status,ordertype , playDate,slotFrom,slotTo  , Amount   , Isnull( PaymentStatus,0)  PaymentStatus  , isnull( PaymentReference,'') PaymentReference , isnull( TransactionID  ,'') TransactionID
                           from #O INNER JOIN #OS ON #O.SNO=#OS.SNO where status = 'new'
                           " + dates + "order by OrderSort";

            return JsonConvert.SerializeObject(dl.LoadData(qry));

        }

        public string ViewOrdersPending(string id)
        {
            DataLogic dl = new DataLogic(_configuration);

            String qry = @"SELECT O.ORDERID AS id, O.ORDERNO AS orderNo, O.VISITORID AS visitorid, O.DATE AS date,
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
                           
                           WHERE O.STATUS = 'new' AND O.ORDERNO = '" + id +"' ORDER BY O.ORDERID;";

            String qry2 = @"SELECT DISTINCT OS.SLOTFROM AS slotFrom, OS.SLOTTO AS slotTo, OS.PLAYDATE AS playDate
                           FROM TBLORDERS O
						   INNER JOIN TBLORDERSCHEDULE OS ON OS.VISITORID = O.VISITORID
                           
                           WHERE O.STATUS = 'new' AND O.ORDERNO = '" + id + "' ORDER BY OS.PLAYDATE;";


            var dt1 = dl.LoadData(qry);
            var dt2 = dl.LoadData(qry2);

            var data = new
            {
                detail = dt1,
                slots = dt2

            };

            return JsonConvert.SerializeObject(data);
        }


        public IActionResult SaveOrderPending(string orderNo, string status, string emailSent, string message)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    if (orderNo != null)
                    {

                        var orders = _context.TblOrders.Where(x => x.OrderNo == orderNo).ToList();

                        if (orders != null && orders.Any())
                        {
                            // Update the status for all matching orders
                            foreach (var order in orders)
                            {
                                order.Status = status;
                            }

                            _context.TblOrders.UpdateRange(orders);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        return Json(false);
                    }
                    transaction.Commit();

                    if (emailSent != null)
                    {
                        _email.SendEmail(emailSent, "Order " + orderNo + " Update", message);
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }

            return Json(true);
        }


        #endregion

        #region Cart Order


        public IActionResult OrderCart()
        {
            return View();
        }


        public string GetOrdersCart(string OrderDate, string SlotDate)
        {
            DataLogic dl = new DataLogic(_configuration);

            string dates = "";

            if (SlotDate != null)
            {
                dates = " and PlayDate = '" + SlotDate + "' ";
            }

            if (OrderDate != null)
            {
                dates = " and date = '" + OrderDate + "' ";
            }

            if (SlotDate != null && OrderDate != null)
            {
                dates = " and date = '" + OrderDate + "' and PlayDate = '" + SlotDate + "' ";
            }

            if (SlotDate == null && OrderDate == null)
            {
                dates = "";
            }

            String qry = @"select distinct 1 Sno, visitorId,DATE,STATUS,ORDERTYPE
                           INTO #O
                           from Tblorders
                           select top 1 1 Sno, Playdate,SlotFrom,Slotto
                           INTO #OS
                           from TblOrderSchedule order by Playdate asc
                           select CONVERT(BIGINT,dbo.ExtractNumericValues(visitorId))
                           OrderSort,visitorId,date,status,ordertype , playDate,slotFrom,slotTo
                           from #O LEFT JOIN #OS ON #O.SNO=#OS.SNO where status = 'cart'
                           " + dates + " order by [Date]";

            return JsonConvert.SerializeObject(dl.LoadData(qry));
        }

        public string ViewOrdersCart(string id)
        {
            DataLogic dl = new DataLogic(_configuration);

            String qry = @"SELECT O.ORDERID AS id, O.VISITORID AS visitorid, O.DATE AS date,
                           C.CUSTOMERID AS customerId, C.FIRSTNAME + ' ' + C.SECONDNAME AS customerName,
                           C.EMAIL AS customerEmail, ST.STATIONID AS stationId,
                           ST.STATIONNAME AS stationName, SC.SCREENSIZE as size, SC.SCREENID AS screenId,
                           SC.SCREENNAME AS screenName, O.RATE AS rate, O.AMOUNT AS amount,
                           O.STATUS AS status,
                           O.ORDERIMAGE AS image, ISNULL(O.PAYMENTMETHOD,'') AS paymentMethod, O.ORDERTYPE AS ordertype
                           
                           FROM TBLORDERS O
                           LEFT OUTER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = O.CUSTOMERID
                           LEFT OUTER JOIN TBLSCREENS SC ON SC.SCREENID = O.SCREENID
                           LEFT OUTER JOIN TBLSTATIONS ST ON ST.STATIONID = O.STATIONID
                           
                           WHERE O.STATUS = 'cart' AND O.VISITORID = '" + id + "' ORDER BY O.ORDERID;";

            String qry2 = @"SELECT DISTINCT OS.SLOTFROM AS slotFrom, OS.SLOTTO AS slotTo, OS.PLAYDATE AS playDate
                           FROM TBLORDERS O
						   LEFT OUTER JOIN TBLORDERSCHEDULE OS ON OS.VISITORID = O.VISITORID
                           
                           WHERE O.STATUS = 'cart' AND O.VISITORID = '" + id + "' ORDER BY OS.PLAYDATE;";


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


        #region EditOrderImage
        public string GetCartbyCustomer(string orderNo)
        {

            DataLogic dl = new DataLogic(_configuration);

            var csid = new SessionData(_httpContextAccessor).GetData();

            string qry = "Select c.ScreenId,c.VisitorId,sc.ScreenName, c.orderimage, c.text1, c.text2, c.text3, c.Text1Color, c.Text2Color, c.Text3Color,sc.ScreenSize, sc.Rate, st.StationId, st.StationName,\r\nc.text1font, c.text2font, c.text3font, c.Text1TopPosition, c.Text1LeftPosition, c.Text2TopPosition, c.Text2LeftPosition,c.Text3TopPosition, c.Text1Size,c.Text2Size,c.Text3Size, c.Text3LeftPosition  from TblOrders c\r\nLeft Join TblScreens sc On sc.screenid = c.screenId\r\nLeft Join tblStations st On st.stationid = sc.stationid where c.orderno = '" + orderNo + "'";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }


        public IActionResult EditOrderImage()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> EditDesign([FromBody] SaveDesignRequest request, [FromQuery] string OrderNo)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                 
                    foreach (var screenData in request.ScreenDataArray)
                    {
                        var screenId = screenData.ScreenId;
                        var screen = await _context.TblOrders.FirstOrDefaultAsync(u => u.OrderNo == OrderNo && u.ScreenId == screenId);
                        if (screen != null)
                        {
                            screen.Text1LeftPosition = screenData.LeftValue1;
                            screen.Text2LeftPosition = screenData.LeftValue2;
                            screen.Text3LeftPosition = screenData.LeftValue3;
                            screen.Text1TopPosition = screenData.TopValue1;
                            screen.Text2TopPosition = screenData.TopValue2;
                            screen.Text3TopPosition = screenData.TopValue3;


                            _context.TblOrders.Update(screen);
                        }
                    }

                    await _context.SaveChangesAsync();
                    scope.Complete(); // Commit the transaction

                    return Ok("Saved");
                }
                catch (Exception ex)
                {
                    // Handle the exception, e.g., log or return an error response
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
                finally
                {
                    scope.Dispose();
                }
            }
        }


        #endregion

    }
}
