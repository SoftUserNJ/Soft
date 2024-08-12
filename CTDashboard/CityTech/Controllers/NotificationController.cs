using System;
using System.IO;
using System.Net;
using System.Text;
using CityTech.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityTech.Controllers
{
    public class NotificationController : Controller
    {
        private const string ApplicationId = "AAAAQp3mfFs:APA91bGg3hnw0zrkEC1b2ZQL6FB1vn1oMDASXjeRrkazCNxPvfPalmUhiPpXKiKOWnzqBkcRDrAVJFT37X7Rr-Oqj2297rrYCSjMYLWGoTtwGx4cgGbjhRif6qCADgNn57O3nP8qs3JK";
        private const string SenderId = "286116969563";

        //public IActionResult SendNotification<T>(string deviceId, int incidentNo, T modelObject, string body, string title, string icon, TblIncident updateIndidnet)
        //{
        public IActionResult SendNotification(string deviceId, int incidentNo , TblIncident updateIndidnet, string body, string title, string icon)
        {
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data1 = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = body,
                        title = title,
                        icon = icon
                     
                       
                    },
                   // Include your model object in the data field
                };

                var json = JsonConvert.SerializeObject(data1);
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add($"Authorization: key={ApplicationId}");
                tRequest.Headers.Add($"Sender: id={SenderId}");
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = tReader.ReadToEnd();

                                // Check the HTTP status code of the response
                                var httpResponse = tResponse as HttpWebResponse;
                                if (httpResponse != null)
                                {
                                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        // The notification was sent successfully
                                        return Ok(sResponseFromServer);
                                    }
                                    else
                                    {
                                        // Handle client errors or server errors
                                        // You can extract the error message from sResponseFromServer
                                        // For example, if FCM returns error details in JSON format:
                                        // You can deserialize it and return as a BadRequest result.
                                        try
                                        {
                                            var errorDetails = JsonConvert.DeserializeObject<Plobj>(sResponseFromServer);
                                            return BadRequest(errorDetails.Message);
                                        }
                                        catch (Exception)
                                        {
                                            // If the response cannot be deserialized as Plobj, return a generic error message.
                                            return BadRequest("Failed to send notification. Server returned an error.");
                                        }
                                    }
                                }
                                else
                                {
                                    // Handle the case when the response is not a valid HTTP response
                                    return BadRequest("Invalid response from FCM server.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception, e.g., logging or error reporting.
                return BadRequest(ex.Message);
            }
        }
    }

    // Add the Plobj class here if it's not already defined.
    public class Plobj
    {
        public string Message { get; set; }
        public string TagMsg { get; set; }
    }
}
