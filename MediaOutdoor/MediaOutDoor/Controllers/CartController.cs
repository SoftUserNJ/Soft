using CityTechWEBAPI;
using MediaOutdoor.Models;
using MediaOutDoor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Transactions;

namespace MediaOutDoor.Controllers
{
    public class CartController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly MediaOutdoorContext _dbContext;

        public CartController(IConfiguration configuration, MediaOutdoorContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }



        public IActionResult SaveCart(int screenId, string VisitorId, int stationId,float rate, string? orderType = null)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    DateTime currentDate = DateTime.Now.Date;
                    var maxOrderNo = _dbContext.TblOrders.Max(x => (int?)x.OrderId) ?? 0;
                    var maxCartid = _dbContext.TblOrders.Max(x => (int?)x.CartId) ?? 0;
                    var OrderNo = maxOrderNo + 1;
                    var CartId = maxCartid + 1;
                    var cart = new TblOrder()
                    {
                        OrderId = OrderNo,
                        VisitorId = VisitorId,
                        ScreenId = screenId,
                        Status = "Cart",
                        StationId = stationId,
                        CartId = CartId,
                        Rate = rate,
                        Date = currentDate,
                        OrderType = orderType ?? "B2C"
                    };
                    _dbContext.TblOrders.Add(cart);
                    _dbContext.SaveChanges();

                    // Commit the transaction
                    scope.Complete();

                    return Json("Cart Saved");
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on exception
                    scope.Dispose(); // This will perform a rollback
                    return Json("An error occurred while saving the cart: " + ex.Message);
                }
            }
        }



        [HttpPost]
        public IActionResult RemoveScreenFromCart(int screenId, string visitorId)
        {
            try
            {
               
                var cartItem = _dbContext.TblOrders.FirstOrDefault(c => c.ScreenId == screenId && c.VisitorId == visitorId);

                if (cartItem != null)
                {
                    _dbContext.TblOrders.Remove(cartItem);
                    _dbContext.SaveChanges();
                    return Json("Screen removed from cart");
                }
                else
                {
                    return Json("Screen not found in the cart");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IActionResult Cart()
        {
            return View();
        }

      


        public string GetCartbyVisitor(string VisitorId)
        {
            
            DataLogic dl = new DataLogic(_configuration);

            string qry = "Select  os.slotfrom, os.slotto, os.playdate, c.ScreenId,c.VisitorId,sc.ScreenName, sc.ScreenSize, c.Rate, st.StationId, st.StationImage, st.StationName from TblOrders c\r\nLeft Join TblScreens sc On sc.screenid = c.screenId\r\nLeft Join tblStations st On st.stationid = sc.stationid \r\nLeft Join TblOrderSchedule os On os.visitorid = c.visitorid\r\nwhere c.visitorid ='" + VisitorId + "'";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }


        [HttpPost]
        public string RemoveStationFromCart(int stationId, string visitorId)
        {
            DataLogic dl = new DataLogic(_configuration);

            string qry = "Delete from TblOrders where StationId ='" + stationId + "' And VisitorId = '" + visitorId + "'";
            string qry2 = "Delete from tblorderschedule where VisitorId = '" + visitorId + "'";

            var dt = dl.LoadData(qry);
            var dt2 = dl.LoadData(qry2);

            return JsonConvert.SerializeObject(dt);
        }

        public IActionResult CheckDesign(string visitorId)
        { 
            var matchingOrders = _dbContext.TblOrders.Where(order => order.VisitorId == visitorId).ToList();

            bool hasNullImage = matchingOrders.Any(order => order.OrderImage == null);

            return Json(!hasNullImage);
        }


    }
}
