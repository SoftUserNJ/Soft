using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace CityTechWEBAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmailController : ControllerBase
	{
		private static readonly string BASE_URL = "https://qgzggm.api.infobip.com";
		private static readonly string API_KEY = "9308196cad6a7daf66f4d6ceebe832d4-9091fc18-3e31-47e8-9ce0-176ad42e2179";
		private static readonly string SENDER_EMAIL = "nouman@softaxe.com";
		private static readonly string RECIPIENT_EMAIL = "info@citytech.nl";

		private static readonly string EMAIL_SUBJECT = "This is a sample email subject";
		private static readonly string EMAIL_TEXT = "This is a sample email message.";


		[HttpPost]
		[Route("send")]
		public async Task<IActionResult> SendEmail()
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri(BASE_URL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("App", API_KEY);

				var request = new MultipartFormDataContent();
				request.Add(new StringContent(SENDER_EMAIL), "from");
				request.Add(new StringContent(RECIPIENT_EMAIL), "to");
				request.Add(new StringContent(EMAIL_SUBJECT), "subject");
				request.Add(new StringContent(EMAIL_TEXT), "text");

				try
				{
					var response = await client.PostAsync("email/2/send", request);

					var responseContent = response.Content;
					string responseString = await responseContent.ReadAsStringAsync();
					var responseCode = response.StatusCode;

					return Ok(new
					{
						StatusCode = responseCode,
						Response = responseString
					});
				}
				catch (Exception e)
				{
					return BadRequest(new
					{
						Error = e.Message
					});
				}
			}
		}
	}
}
