using CityTechWEBAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityTechWEBAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VoiceCallController : ControllerBase
	{
		private readonly InfoBipCallService _infoBipCallService;
		public VoiceCallController(InfoBipCallService infoBipCallService)
		{
			_infoBipCallService = infoBipCallService;
		}

		[HttpPost]
		public IActionResult SendVoiceCall()
		{
			_infoBipCallService.SendVoiceCall();
			return Ok("Voice call sent.");
		}
	}
}
