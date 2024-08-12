using Microsoft.AspNetCore.Mvc;
using YourNamespace.Services;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsappController : ControllerBase
    {
        private readonly IWhatsappService _whatsappService;

        public WhatsappController(IWhatsappService whatsappService)
        {
            _whatsappService = whatsappService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage()
        {
            var response = await _whatsappService.SendMessageAsync();
            return Ok(response);
        }
    }
}
