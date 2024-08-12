using Microsoft.AspNetCore.Mvc;

namespace MediaOutDoor.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Payment()
        {
            return View();
        }
    }
}
