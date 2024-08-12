using RestSharp;
using System;
using System.Collections.Generic; // Add this namespace for List<T>

namespace CityTechWEBAPI.Services
{
	public class InfoBipCallService
	{
		private readonly RestClient _client;

		public InfoBipCallService()
		{
			_client = new RestClient("https://qgzggm.api.infobip.com");
		}

		public void SendVoiceCall()
		{
			var request = new RestRequest("/tts/3/advanced", Method.Post);
			request.AddHeader("Authorization", "App 9308196cad6a7daf66f4d6ceebe832d4-9091fc18-3e31-47e8-9ce0-176ad42e2179");
			request.AddHeader("Content-Type", "application/json");
			request.Timeout = -1;

			var body = @"{
                ""messages"": [{
                    ""from"": ""923230110083"",
                    ""destinations"": [{
                        ""to"": ""923230110083""
                    }],
                    ""text"": ""Hello, this is your voice message. This is Ammar How are You, Are You Fine?""
                }],
                ""applicationId"": ""ACTUAL_APPLICATION_ID""
            }";

			request.AddParameter("application/json", body, ParameterType.RequestBody);

			RestResponse response = _client.Execute(request);

			Console.WriteLine(response.Content);
		}
	}
}