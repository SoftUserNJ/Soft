using CityTechWEBAPI.Models;
using System.Net.Http;

namespace CityTechWEBAPI.Services
{
	public class InfoBipSmsService : ISmsService
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiKey;
		private readonly string _baseUrl = "https://qgzggm.api.infobip.com";


		public InfoBipSmsService(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_apiKey = configuration["InfoBipSms:ApiKey"];
		}


		public async Task<bool> SendSmsAsync(string phoneNumber, string message)
		{
			var payload = new
			{
				from = "SENDER_NAME",
				to = phoneNumber,
				text = message
			};

			_httpClient.DefaultRequestHeaders.Add("Authorization", $"App {_apiKey}");
			var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/sms/2/text/single", payload);

			return response.IsSuccessStatusCode;
		}

		
	}
	
}
