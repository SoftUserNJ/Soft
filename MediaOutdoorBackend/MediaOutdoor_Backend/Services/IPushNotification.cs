using System;
using System.IO;
using System.Net;
using System.Text;
using MediaOutdoor_Backend.Models;
using MediaOutdoor_Backend.Sevices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MediaOutdoor_Backend.Services
{

    public interface IPushNotification
    {
        //IActionResult SendPushNotification(string deviceId, string body, string title, string icon);
        void SendPushNotificationNew(string deviceId, string body, string title, string icon);
    }


    public class PushNotification : IPushNotification
    {
        private const string ApplicationId = "AAAAR29kzgg:APA91bEkkxE6tbPAsZn44H4fsfh58Iq0UOP9VrVDmbOqwRnF3WvusP2mh3TsnbkokMuKTWAW5X1TklzjcjZRI_3_g9gXqvYTtBGvvSpMzRcRoph-TiJr5XhdWANFnIFyaD4TnB-Bb-1E";
        private const string SenderId = "306811555336";
        private readonly MediaOutdoorContext _context;
        private readonly IConfiguration _configuration;


        public PushNotification(MediaOutdoorContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void SendPushNotificationNew(string deviceId, string body, string title, string icon)
        {
            //if (title.Length > 0)
            //{
            //    Console.WriteLine("Push Call Working in schedule message");

            //}

            //DataLogic dl = new DataLogic(_configuration);
            //String qry = @"SELECT fcmtoken FROM TblCustomers WHERE fcmtoken IS NOT NULL;";
            //var fcmTokennew = JsonConvert.SerializeObject(dl.LoadData(qry));


            var fcmToken = _context.TblCustomers.Where(x => x.Fcmtoken != null).Select(s => s.Fcmtoken).ToList();

            if (fcmToken.Count == 0)
            {
                throw new Exception("Fcm Token Not Found");
            }

            foreach (var item in fcmToken)
            {
                try
                {
                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data1 = new
                    {
                        to = item,
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
                                            Console.WriteLine("Status Ok");
                                        }
                                        else
                                        {

                                            try
                                            {
                                                var errorDetails = JsonConvert.DeserializeObject<Plobj>(sResponseFromServer);
                                                throw new Exception(errorDetails.Message);

                                            }
                                            catch (Exception ex)
                                            {
                                                // If the response cannot be deserialized as Plobj, return a generic error message.
                                                throw new Exception("Failed to send notification. Server returned an error.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // Handle the case when the response is not a valid HTTP response
                                        throw new Exception("Invalid response from FCM server.");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception, e.g., logging or error reporting.
                    throw new Exception(ex.Message);
                }

            }
        }


        //public IActionResult SendPushNotification(string deviceId, string body, string title, string icon)
        //{
        //    var fcmToken = _context.TblCustomers.Where(x => x.Fcmtoken != null).Select(s => s.Fcmtoken).ToList();

        //    if (fcmToken == null || fcmToken.Count == 0)
        //    {
        //        throw new Exception("Fcm Token Not Found");
        //    }

        //    foreach (var item in fcmToken)
        //    {
        //        try
        //        {
        //            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //            tRequest.Method = "post";
        //            tRequest.ContentType = "application/json";
        //            var data1 = new
        //            {
        //                to = item,
        //                notification = new
        //                {
        //                    body = body,
        //                    title = title,
        //                    icon = icon
        //                },
        //                // Include your model object in the data field
        //            };

        //            var json = JsonConvert.SerializeObject(data1);
        //            byte[] byteArray = Encoding.UTF8.GetBytes(json);
        //            tRequest.Headers.Add($"Authorization: key={ApplicationId}");
        //            tRequest.Headers.Add($"Sender: id={SenderId}");
        //            tRequest.ContentLength = byteArray.Length;
        //            using (Stream dataStream = tRequest.GetRequestStream())
        //            {
        //                dataStream.Write(byteArray, 0, byteArray.Length);
        //                using (WebResponse tResponse = tRequest.GetResponse())
        //                {
        //                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
        //                    {
        //                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
        //                        {
        //                            string sResponseFromServer = tReader.ReadToEnd();

        //                            // Check the HTTP status code of the response
        //                            var httpResponse = tResponse as HttpWebResponse;
        //                            if (httpResponse != null)
        //                            {
        //                                if (httpResponse.StatusCode == HttpStatusCode.OK)
        //                                {
        //                                    // The notification was sent successfully
        //                                    throw new Exception(sResponseFromServer);
        //                                }
        //                                else
        //                                {
        //                                    // Handle client errors or server errors
        //                                    // You can extract the error message from sResponseFromServer
        //                                    // For example, if FCM returns error details in JSON format:
        //                                    // You can deserialize it and return as a BadRequest result.
        //                                    try
        //                                    {
        //                                        var errorDetails = JsonConvert.DeserializeObject<Plobj>(sResponseFromServer);
        //                                        throw new Exception(errorDetails.Message);

        //                                    }
        //                                    catch (Exception ex)
        //                                    {
        //                                        // If the response cannot be deserialized as Plobj, return a generic error message.
        //                                        throw new Exception("Failed to send notification. Server returned an error.");
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                // Handle the case when the response is not a valid HTTP response
        //                                throw new Exception("Invalid response from FCM server.");
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exception, e.g., logging or error reporting.
        //            throw new Exception(ex.Message);
        //        }

        //    }
        //    throw new Exception();
        //}

    }

    public class Plobj
    {
        public string Message { get; set; }
        public string TagMsg { get; set; }
    }



}
