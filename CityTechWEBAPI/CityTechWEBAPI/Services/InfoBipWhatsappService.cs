using RestSharp;
using System.Threading.Tasks;

namespace YourNamespace.Services
{
    public interface IWhatsappService
    {
        Task<string> SendMessageAsync();
    }

    public class InfoBipWhatsappService : IWhatsappService
    {
        public async Task<string> SendMessageAsync()
        {
            var client = new RestClient("https://qgzggm.api.infobip.com");
           

            var request = new RestRequest("/whatsapp/1/message/text", Method.Post);
            request.AddHeader("Authorization", "App 9308196cad6a7daf66f4d6ceebe832d4-9091fc18-3e31-47e8-9ce0-176ad42e2179");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.Timeout = -1;
            var body = @"{""from"":""923230110083"",""to"":""441134960001"",""messageId"":""a28dd97c-1ffb-4fcf-99f1-0b557ed381da"",""content"":{""text"":""Some text""},""callbackData"":""Callback data"",""notifyUrl"":""https://www.example.com/whatsapp"",""urlOptions"":{""shortenUrl"":true,""trackClicks"":true,""trackingUrl"":""https://example.com/click-report"",""removeProtocol"":true,""customDomain"":""example.com""}}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            return response.Content;
        }
    }
}
