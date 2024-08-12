
using CityTech.Models;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MailKit;
using CityTech.UserModels;
using System.Text.RegularExpressions;
using System.Transactions;
using CityTech;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CityTech.VMs;
using Newtonsoft.Json;
using CityTech.Sevices;
using Microsoft.Extensions.Configuration;
using System.Globalization;

public class EmailBackgroundService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EmailBackgroundService> _logger;
    private readonly IHubContext<EmailHub> _hubContext;
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string _baseUrl = "https://qgzggm.api.infobip.com";
    public EmailBackgroundService(IServiceProvider serviceProvider, ILogger<EmailBackgroundService> logger, IHubContext<EmailHub> hubContext, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _hubContext = hubContext;
        _configuration = configuration;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CityTechContext>();
                string hostname = "imap.one.com";
                int port = 993;
                string username = "controller@citytech.nl";
                string password = "CityTROLL23#$";
                List<EmailColumnDetail> listemailcoulmn = new List<EmailColumnDetail>
        {

            new EmailColumnDetail { BaseLanguage = "Glasbreuk Sirene:",        EnglishLanguage = "GlassBreakSiren" },
            new EmailColumnDetail { BaseLanguage = "Relais 2:",                EnglishLanguage = "Relay2" },
            new EmailColumnDetail { BaseLanguage = "Glasbreuk:",               EnglishLanguage = "GlassBreak" },
            new EmailColumnDetail { BaseLanguage = "Glasbreuk Frequency:",     EnglishLanguage = "GlassBreakFrequency" },
            new EmailColumnDetail { BaseLanguage = "Deur Contact:",            EnglishLanguage = "DoorContact" },
            new EmailColumnDetail { BaseLanguage = "Deur Contact Frequency:",  EnglishLanguage = "DoorContactFrequency" },
            new EmailColumnDetail { BaseLanguage = "leeg:",                    EnglishLanguage = "Empty" },
            new EmailColumnDetail { BaseLanguage = "leeg 2:",                  EnglishLanguage = "Empty2" },
            new EmailColumnDetail { BaseLanguage = "Relais 4:",                EnglishLanguage = "Relay4" },
            new EmailColumnDetail { BaseLanguage = "Relais 3:",                EnglishLanguage = "Relay3" },
            new EmailColumnDetail { BaseLanguage = "temperatuur sensor:",      EnglishLanguage = "TemperatureSensor" },
            new EmailColumnDetail { BaseLanguage = "Vin:",                     EnglishLanguage = "Vin" },
            new EmailColumnDetail { BaseLanguage = "Time:",                    EnglishLanguage = "Time" },
            new EmailColumnDetail { BaseLanguage = "Router:",                  EnglishLanguage = "Router" },
            new EmailColumnDetail { BaseLanguage = "sirene:",                  EnglishLanguage = "Siren" },
            new EmailColumnDetail { BaseLanguage = "Glasbreuk/deur:",          EnglishLanguage = "GlassBreakDoor" },
            new EmailColumnDetail { BaseLanguage = "Glasbreuk/deur Frequency:",EnglishLanguage = "GlassBreakDoorFrequency" },
            new EmailColumnDetail { BaseLanguage = "motion detected:",         EnglishLanguage = "MotionDetected" },
            new EmailColumnDetail { BaseLanguage = "motion detected Count:",   EnglishLanguage = "MotionDetectedCount" },
            new EmailColumnDetail { BaseLanguage = "Spanning na automaat:",    EnglishLanguage = "Voltageafterthecircuitbreaker" },
            new EmailColumnDetail { BaseLanguage = "Spanning voor automaat:",  EnglishLanguage = "Voltagebeforethecircuitbreaker" },
            new EmailColumnDetail { BaseLanguage = "Master Screen:",           EnglishLanguage = "MasterScreen" },
            new EmailColumnDetail { BaseLanguage = "Player:",                  EnglishLanguage = "Player" }
    };



                //string phoneNumber = "+923230110083";
                //string message1 = "SHAHZAD SB";

                //bool result = await SendSmsAsync(_configuration, phoneNumber, message1);

                //if (result)
                //{
                //    // SMS was sent successfully
                //}
                //else
                //{
                //    // SMS sending failed
                //}
                
                
                //  goto IStoreLabelsRequest;
                using (var client = new ImapClient())
                {
                    client.Connect(hostname, port, SecureSocketOptions.SslOnConnect);
                    client.Authenticate(username, password);
                    client.Inbox.Open(FolderAccess.ReadWrite);
                    var uids = client.Inbox.Search(SearchQuery.NotSeen);
                    foreach (var uid in uids)
                    {
                        string postalCode = "";
                        string stationName = "";
                        string locationName = "";
                        string objectName = "";
                        string IncdientName = "";
                        int IncidentTypeId = 0;
                        TblObject ObjectTable = null;


                        var Item = listemailcoulmn.Select(item => new EmailColumnDetail
                        {
                            BaseLanguage = item.BaseLanguage,
                            EnglishLanguage = item.EnglishLanguage,
                            Value = item.Value
                        }).ToList();
                        var message = client.Inbox.GetMessage(uid);
                        string subject = message.Subject;
                        string sender = message.From.ToString();
                        string body = message.TextBody;
                        //string subject = "EXTERNAL - 1021HJ-NDP-002-PLA-002 - Trigger: High Temperature";
                        if (subject.Contains("Trigger"))
                        {
                            string pattern = @"EXTERNAL - (\w+)-(\w+)-([\w-]+)-(\w+) - Trigger: (.+)";
                            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                            Match match = regex.Match(subject);
                            if (match.Success)
                            {
                                postalCode = match.Groups[1].Value;
                                stationName = match.Groups[2].Value;
                                locationName = match.Groups[3].Value;
                                objectName = match.Groups[4].Value;
                                IncdientName = match.Groups[5].Value;
                                IncidentTypeId = dbContext.TblIncidentTypes
                             .Where(i => i.IncidentName == IncdientName)
                             .Select(i => i.IncidentTypeId)
                             .SingleOrDefault();

                                ObjectTable = dbContext.TblObjects.Where(i => i.ObjectName == objectName).SingleOrDefault();
                            }

                            Dictionary<string, string> baseLanguageMappings = Item.ToDictionary(i => i.BaseLanguage.TrimEnd(':'), i => i.EnglishLanguage);
                            foreach (var line in body.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                int colonIndex = line.IndexOf(':');
                                if (colonIndex >= 0)
                                {
                                    string attributeName = line.Substring(0, colonIndex).Trim();
                                    if (baseLanguageMappings.TryGetValue(attributeName, out var englishAttributeName))
                                    {
                                        string attributeValue = line.Substring(colonIndex + 1).Trim();
                                        var item = Item.FirstOrDefault(i => i.BaseLanguage.TrimEnd(':') == attributeName);
                                        if (item != null)
                                        {
                                            item.Value = attributeValue;
                                        }
                                    }
                                }
                            }


                            using (var dbContext1 = new CityTechContext())
                            {
                                using (var scope1 = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                                {
                                    try
                                    {

                                        var incidentdata = dbContext.TblIncidents.Where(x => x.Emailno.Equals(uid.ToString())).FirstOrDefault();
                                        if (incidentdata != null)
                                        {
                                            dbContext.TblIncidents.Remove(incidentdata);
                                        }
                                        int newIncidentNo = 0;
                                        int? maxIncidentNo = dbContext.TblIncidents.Max(i => (int?)i.IncidentNo);
                                        if (maxIncidentNo.HasValue)
                                        {
                                            newIncidentNo = maxIncidentNo.Value + 1;
                                        }
                                        else
                                        {

                                            newIncidentNo = 1;
                                        }
                                        string formattedIncidentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        DateTime formattedDateTime = DateTime.ParseExact(formattedIncidentDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                        string formattedString = formattedDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                        var incident = new TblIncident
                                        {
                                            IncidentNo = newIncidentNo,
                                            IncidentDate = formattedDateTime,
                                            IncidentTypeId = IncidentTypeId,
                                            ObjectId = ObjectTable.ObjectId,
                                            LocId = ObjectTable.LocId,
                                            MechanicId = 0,
                                            Prepration = "",
                                            Requirement = "",
                                            Emailno = uid.ToString(),
                                            GlassBreakSiren = Item.FirstOrDefault(x => x.EnglishLanguage == "GlassBreakSiren")?.Value,
                                            Relay2 = Item.FirstOrDefault(x => x.EnglishLanguage == "Relay2")?.Value,
                                            GlassBreak = Item.FirstOrDefault(x => x.EnglishLanguage == "GlassBreak")?.Value,
                                            GlassBreakFrequency = Item.FirstOrDefault(x => x.EnglishLanguage == "GlassBreakFrequency")?.Value,
                                            DoorContact = Item.FirstOrDefault(x => x.EnglishLanguage == "DoorContact")?.Value,
                                            Empty = Item.FirstOrDefault(x => x.EnglishLanguage == "Empty")?.Value,
                                            Empty2 = Item.FirstOrDefault(x => x.EnglishLanguage == "Empty2")?.Value,
                                            Relay4 = Item.FirstOrDefault(x => x.EnglishLanguage == "Relay4")?.Value,
                                            Relay3 = Item.FirstOrDefault(x => x.EnglishLanguage == "Relay3")?.Value,
                                            TemperatureSensor = Item.FirstOrDefault(x => x.EnglishLanguage == "TemperatureSensor")?.Value,
                                            Vin = Item.FirstOrDefault(x => x.EnglishLanguage == "Vin")?.Value,
                                            Time = Item.FirstOrDefault(x => x.EnglishLanguage == "Time")?.Value,
                                            Router = Item.FirstOrDefault(x => x.EnglishLanguage == "Router")?.Value,
                                            Siren = Item.FirstOrDefault(x => x.EnglishLanguage == "Siren")?.Value,
                                            GlassBreakDoor = Item.FirstOrDefault(x => x.EnglishLanguage == "GlassBreakDoor")?.Value,
                                            GlassBreakDoorFrequency = Item.FirstOrDefault(x => x.EnglishLanguage == "GlassBreakDoorFrequency")?.Value,
                                            MotionDetected = Item.FirstOrDefault(x => x.EnglishLanguage == "MotionDetected")?.Value,
                                            MotionDetectedCount = Item.FirstOrDefault(x => x.EnglishLanguage == "MotionDetectedCount")?.Value,
                                            Voltageafterthecircuitbreaker = Item.FirstOrDefault(x => x.EnglishLanguage == "Voltageafterthecircuitbreaker")?.Value,
                                            Voltagebeforethecircuitbreaker = Item.FirstOrDefault(x => x.EnglishLanguage == "Voltagebeforethecircuitbreaker")?.Value,
                                            MasterScreen = Item.FirstOrDefault(x => x.EnglishLanguage == "MasterScreen")?.Value,
                                            Player = Item.FirstOrDefault(x => x.EnglishLanguage == "Player")?.Value
                                        };
                                        dbContext.TblIncidents.Add(incident);
                                        await dbContext.SaveChangesAsync();
                                        client.Inbox.AddFlags(uid, MessageFlags.Seen, true);
                                        scope1.Complete();
                                        await _hubContext.Clients.All.SendAsync("ReceiveIncidentSchedule", 0);

                                    }
                                    catch (Exception ex)
                                    {
                                        // Handle exception or log error
                                    }
                                }
                            }



                        }
                    }

                    client.Inbox.Close();
                    client.Disconnect(true);
                }

            IStoreLabelsRequest:





                //var unscheduledIncidents = await dbContext.TblIncidents.Where(incident => !incident.IsScheduled).ToListAsync();
                //foreach (var incident in unscheduledIncidents)
                //{

                DataLogic dl = new DataLogic(_configuration);
                String qry1 = @"SELECT ITYPES.INCIDENTNAME AS INCIDENT, I.INCIDENTNO, L.LOCNAME AS LOCATION,
                            ITYPES.PREPRATION, ITYPES.REQUIREMENTS, O.OBJECTNAME AS OBJECT, ITYPES.SLARESPONSE,
                            ITYPES.SLASECURE, ITYPES.SLAFIXED, ITYPES.PRIOTYPE , ISNULL(IW.MechanicId,0) MECHANICID, (CASE WHEN ISNULL(IW.WORKSNO,0)  >0 THEN 'REJECTED' ELSE 'NEW' END) STATUS  , ISNULL(IW.WORKDES,'') WORKDES
                         
                            FROM TBLINCIDENTS I
                            LEFT OUTER JOIN TblIncidentWork IW ON IW.INCIDENTNO=I.INCIDENTNO  AND IW.WORKSNO=(SELECT MAX(WORKSNO) FROM  TblIncidentWork IWW WHERE  ISNULL(IWW.WORKSTATUS,'')='REJECTED'  AND  IWW.INCIDENTNO=IW.INCIDENTNO ) AND ISNULL(IW.WORKSTATUS,'')='REJECTED'
                            INNER JOIN TBLINCIDENTTYPES ITYPES ON ITYPES.INCIDENTTYPEID = I.INCIDENTTYPEID  
                            INNER JOIN TBLOBJECTS O ON O.OBJECTID = I.OBJECTID
                            INNER JOIN TBLOBJECTLOCATIONS L ON L.LOCID = O.LOCID
                            WHERE isnull(I.ISSCHEDULED,0) = 0   ";


                String qry2 = @"Select  DISTINCT  U.USERID, U.USERNAME MECHANIC, INC.INCIDENTNO from TBLINCIDENTS INC
INNER JOIN TblIncidentTypes INCT ON INCT.IncidentTypeId=INC.IncidentTypeId
INNER JOIN TblUserSkills US ON US.SkillId= INCT.SkillId  AND  ISNULL(US.Active,0)=1
INNER JOIN TblUsers U ON U.UserId=US.UserId
INNER JOIN TblUserTypes UT ON UT.UserTypeId=U.UserTypeId
WHERE    isnull(INC.ISSCHEDULED,0) = 0
AND UT.UserType='MECHANIC'  AND isnull(INC.IncidentTag ,'')<>'Mannual' 
union all 

Select  DISTINCT  U.USERID, U.USERNAME MECHANIC, INC.INCIDENTNO from TBLINCIDENTS INC
INNER JOIN TblUsers U ON 1=1
INNER JOIN TblUserTypes UT ON UT.UserTypeId=U.UserTypeId
WHERE    isnull(INC.ISSCHEDULED,0) = 0
AND UT.UserType='MECHANIC'  AND isnull(INC.IncidentTag ,'')='Mannual'  ";



                String qryform = @"select  FORMID , FORMNAME from Tblownform ";


                var dt1 = dl.LoadData(qry1);
                var dt2 = dl.LoadData(qry2);
                var dtqryform = dl.LoadData(qryform);


                var incidentData = new
                {
                    Incident = dt1,
                    Mechanic = dt2,
                    Ownform = dtqryform
                };




                String qry3 = @"SELECT INC.INCIDENTNO AS INCNO, INC.INCIDENTDATE AS INCDATE, OBJ.OBJECTNAME AS OBJECTNAME,
                            OL.LOCNAME AS LOCATION, ITP.INCIDENTNAME AS INCTYPE, U.FIRSTNAME+' '+U.SECONDNAME AS MECHANIC,
                            C.CUSTOMERNAME AS CUSTOMER, INC.ASSIGNEDFIXED AS FIXEDDATE, INC.ASSIGNEDSECURE AS SECUREDATE,
                            INC.WORKEND AS WORKDONEDATE,isnull(INC.ISSCHEDULED,'false') AS ISSCHEDULED, isnull(INC.WORKDONE,'false') AS WORKDONE,
                            isnull(INC.ACCEPTED,'false') AS ACCEPTED, INC.REJECTED AS REJECTED, Isnull(ITP.SLAFIXED,0) SLAFIXED    
                            FROM TBLINCIDENTS INC
                            INNER JOIN TBLOBJECTS OBJ ON OBJ.OBJECTID = INC.OBJECTID
                            INNER JOIN TBLOBJECTLOCATIONS OL ON OBJ.LOCID = OL.LOCID
                            INNER JOIN TBLINCIDENTTYPES ITP ON ITP.INCIDENTTYPEID = INC.INCIDENTTYPEID
                            INNER JOIN TBLUSERS U ON U.USERID = INC.MECHANICID
                            INNER JOIN TBLUSERTYPES UT ON UT.USERTYPEID = U.USERTYPEID
                            INNER JOIN TBLCUSTOMERS C ON C.CUSTOMERID = OBJ.CUSTOMERID  ";

                var dt3 = dl.LoadData(qry3);
                var incidentist = new
                {
                    IncidentList = dt3
                };



                var jsonData = JsonConvert.SerializeObject(incidentData);
                await _hubContext.Clients.All.SendAsync("ReceiveIncidentSchedule", jsonData);
                await _hubContext.Clients.All.SendAsync("IncidentDetailsList", JsonConvert.SerializeObject(dt3));


                //}
            }
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }



   

    public static async Task<bool> SendSmsAsync(IConfiguration configuration, string phoneNumber, string message)
    {
        string apiKey = configuration["InfoBipSms:ApiKey"];

        var payload = new
        {
            from = "SENDER_NAME",
            to = phoneNumber,
            text = message
        };

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"App {apiKey}");
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/sms/2/text/single", payload);

        return response.IsSuccessStatusCode;
    }




}