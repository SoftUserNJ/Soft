using System.Net.Http.Headers;

namespace MediaOutdoor_Backend.Services
{
    public interface IEmail
    {
        void SendEmail(string receiverMail, string emailSubject, string message);
    }


    public class Email : IEmail
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://qgzggm.api.infobip.com";
        private static readonly string SENDER_EMAIL = "nouman@softaxe.com";

        public Email(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["InfoBip:ApiKey"];
        }
       
        public async void SendEmail(string receiverMail, string emailSubject, string message)
        {

            //message = "If you have receive my email please confirm. Regards Noman Aslam";
            //emailSubject = "Important Notice";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("App", _apiKey);

                var request = new MultipartFormDataContent();
                request.Add(new StringContent(SENDER_EMAIL), "from");
                request.Add(new StringContent(receiverMail), "to");
                request.Add(new StringContent(emailSubject), "subject");
                //request.Add(new StringContent(message), "text");
                request.Add(new StringContent(message), "html");

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
