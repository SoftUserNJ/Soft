using MediaOutdoor_Backend.Models;

namespace MediaOutdoor_Backend.Services
{

    public class ScheduleMessage : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public ScheduleMessage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await TimeMatching();
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }


        public async Task TimeMatching()
        {
            DateTime nowDateTime = DateTime.UtcNow.AddHours(5);


            Console.WriteLine("now time is: " + nowDateTime);

            MediaOutdoorContext context = new MediaOutdoorContext();

            var time = context.TblPromotions
                .Where(x => x.Status.ToLower() == "schedule" && x.ScheduleTime != null)
                .Select(s => new TblPromotion
                {
                    Id = s.Id,
                    Message = s.Message,
                    Date = s.Date,
                    Status = s.Status,
                    LastSentDate = s.LastSentDate,
                    App = s.App,
                    Email = s.Email,
                    ScheduleTime = s.ScheduleTime,
                })
                .ToList();


            foreach (var item in time)
            {
                if (item.ScheduleTime < nowDateTime)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var pushNotification = scope.ServiceProvider.GetRequiredService<IPushNotification>();

                        pushNotification.SendPushNotificationNew("", item.Message, "Media Outdoor", "");
                    }

                    Console.WriteLine("Schedule time is: " + item.ScheduleTime + " . And now time is: " + nowDateTime);


                    if (item.Status != null)
                    {
                        var updatedItem = new TblPromotion
                        {
                            Id = item.Id,
                            Message = item.Message,
                            Date = item.Date,
                            Status = "Sent",
                            LastSentDate = item.ScheduleTime,
                            App = item.App,
                            Email = item.Email,
                            ScheduleTime = item.ScheduleTime,
                        };
                        context.Update(updatedItem);
                        context.SaveChanges();
                    }
                }
            }
        }
    }



    //public class ScheduleMessage : BackgroundService
    //{

    //    //private readonly MediaOutdoorContext _context;
    //    //private readonly IPushNotification _pushNotification;
    //    //public ScheduleMessage(MediaOutdoorContext context, IPushNotification pushNotification)
    //    //{
    //    //    _context = context;
    //    //    _pushNotification = pushNotification;
    //    //}

    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {

    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            await TimeMatching();
    //            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

    //        }
    //    }


    //    public async Task TimeMatching()
    //    {

    //        Console.WriteLine("now time is: " + DateTime.UtcNow);

    //        //var time = _context.TblPromotions
    //        //    .Where( x=>x.Status.ToLower() == "schedule")
    //        //    .Select(s=> new {s.Id, s.ScheduleTime, s.Message})
    //        //    .ToList();


    //        //foreach (var item in time)
    //        //{
    //        //    if (item.ScheduleTime < DateTime.UtcNow)
    //        //    {
    //        //        _pushNotification.SendPushNotificationNew("", item.Message, "Media Outdoor", "");

    //        //        Console.WriteLine("Schedule time is: " + item.ScheduleTime + " . And now time is: " + DateTime.UtcNow);
    //        //    }

    //        //}

    //    }
    //}



}
