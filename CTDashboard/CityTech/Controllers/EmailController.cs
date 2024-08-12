using MailKit.Net.Imap;
using MailKit.Security;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using MailKit.Search;

namespace CityTech.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult ReadInbox()
        {
            string hostname = "imap.one.com";
            int port = 993;
            string username = "controller@citytech.nl";
            string password = "CityTROLL23#$";

            using (var client = new ImapClient())
            {
                client.Connect(hostname, port, SecureSocketOptions.SslOnConnect);
                client.Authenticate(username, password);
                client.Inbox.Open(FolderAccess.ReadWrite);
                var uids = client.Inbox.Search(SearchQuery.NotSeen);
                foreach (var uid in uids)
                {
                    var message = client.Inbox.GetMessage(uid);
                    string subject = message.Subject;
                    string sender = message.From.ToString();
                    string body = message.TextBody;
                    client.Inbox.AddFlags(uid, MessageFlags.Seen, true);
                }
                client.Inbox.Close();
                client.Disconnect(true);
            }
            return View();
        }
    }
}
