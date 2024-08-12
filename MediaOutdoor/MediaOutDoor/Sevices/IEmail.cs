using System.Net.Http.Headers;

namespace MediaOutDoor.Services
{
    public interface IEmail
    {
        void SendEmail(string mailFrom, string receiverMail, string emailSubject, string message, string type);
    }


    public class Email : IEmail
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://qgzggm.api.infobip.com";
        private static readonly string SENDER_EMAIL = "shahzad@softaxe.com";

        public Email(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["InfoBip:ApiKey"];
        }
       
        public async void SendEmail(string mailFrom, string mailTo, string emailSubject, string message, string type)
        {
            if (string.IsNullOrEmpty(mailFrom))
            {
                mailFrom = SENDER_EMAIL;
            }

            if (string.IsNullOrEmpty(mailFrom))
            {
                mailTo = SENDER_EMAIL;
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("App", _apiKey);

                var request = new MultipartFormDataContent();
                request.Add(new StringContent(mailFrom), "from");
                request.Add(new StringContent(mailTo), "to");
                request.Add(new StringContent(emailSubject), "subject");
                request.Add(new StringContent(message), type);

                try
                {
                    var response = await client.PostAsync("email/2/send", request);

                    var responseContent = response.Content;
                    string responseString = await responseContent.ReadAsStringAsync();
                    var responseCode = response.StatusCode;

                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }
    }

}
