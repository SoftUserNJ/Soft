using MediaOutdoor_Backend.Models;
using MediaOutdoor_Backend.Sevices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MediaOutdoor_Backend.Controllers
{
    public class InvoicesController : Controller
    {

        private readonly MediaOutdoorContext _context;
        private readonly IConfiguration _configuration;
        private static string order = "";

        public InvoicesController(MediaOutdoorContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public IActionResult OrderInvoice(string orderNo)
        {
            if (string.IsNullOrEmpty(orderNo))
            {
                orderNo = order;
            }
            order = orderNo;
            ViewBag.OrderNoGet = order;
            return View();
        }

        public string ViewOrderInvoice(string orderNo)
        {
            DataLogic dl = new DataLogic(_configuration);

            String qry = @"SELECT O.ORDERNO AS orderNo, O.DATE AS date,
                           C.FIRSTNAME + '' + C.SECONDNAME AS customerName,
                           C.EMAIL AS customerEmail, C.ADDRESS1 AS customerAddress, C.CONTACTNO AS customerContact,
                           ST.STATIONNAME AS stationName, SC.SCREENSIZE as size, 
                           SC.SCREENNAME AS screenName, O.RATE AS rate
                           FROM TBLORDERS O
                           INNER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = O.CUSTOMERID
                           INNER JOIN TBLSCREENS SC ON SC.SCREENID = O.SCREENID
                           INNER JOIN TBLSTATIONS ST ON ST.STATIONID = O.STATIONID
                           
                           WHERE O.ORDERNO = '" + orderNo + "' ORDER BY O.ORDERID;";

            return JsonConvert.SerializeObject(dl.LoadData(qry));
        }

    }
}
