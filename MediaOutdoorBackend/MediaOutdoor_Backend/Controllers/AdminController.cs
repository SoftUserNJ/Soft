using Humanizer;
using ImageMagick;
using MediaOutdoor_Backend.Models;
using MediaOutdoor_Backend.Services;
using MediaOutdoor_Backend.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MediaOutdoor_Backend.Controllers
{
    public class AdminController : Controller
    {


        private readonly MediaOutdoorContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webRoot;
        private readonly IPushNotification _PushNotification;


        public AdminController(MediaOutdoorContext context, IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, IWebHostEnvironment webRoot, IPushNotification pushNotification)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webRoot = webRoot;
            _PushNotification = pushNotification;
        }

        private bool IsPrimaryKeyViolation(DbUpdateException ex)
        {
            // Check if the exception indicates a primary key violation
            // This can vary depending on the database provider you're using
            // You may need to adapt this based on your database provider's error codes or messages
            return ex.InnerException is SqlException sqlException && sqlException.Number == 2627;
        }



        public string GetScreenByStation(string stationId)
        {
            DataLogic dl = new DataLogic(_configuration);

            string qry = "Select sc.ScreenId, sc.StationId, sc.ScreenName, sc.ScreenSize from tblScreens sc where stationid = '" + stationId + "'";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }

        #region Slider

        public IActionResult Sliders()
        {
            return View();
        }

        public IActionResult SaveSliders(int id, string heading, string text, IFormFile image, string htmlStyling, string baseUrl)
        {
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/Sliders/";
            var upload = Path.Combine(webRootPath, exactPath);
            var FileName = "";

            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblSliders.Max(x => (int?)x.SlideNo) ?? 0;
                            id = maxNoUnique + 1;

                            if (image != null)
                            {
                                if (image.Length > 0)
                                {
                                    var extension = Path.GetExtension(image.FileName);
                                    FileName = id + "-" + "hero" + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        image.CopyTo(filesStream);
                                    }
                                }
                            }

                            _context.TblSliders.Add(new TblSlider
                            {
                                SlideNo = id,
                                Heading = heading,
                                Text = text,
                                HtmlStyling = htmlStyling,
                                Image = baseUrl + exactPath + FileName,

                            });

                        }
                        else
                        {
                            var slider = _context.TblSliders.Where(x => x.SlideNo == id).FirstOrDefault();

                            if (image != null)
                            {
                                if (image.Length > 0)
                                {
                                    var extension = Path.GetExtension(image.FileName);
                                    FileName = id + "-" + "hero" + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        image.CopyTo(filesStream);
                                    }
                                }
                            }
                            else
                            {
                                FileName = slider.Image;
                                exactPath = "";
                            }

                            if (slider != null)
                            {

                                slider.Heading = heading;
                                slider.Text = text;
                                slider.HtmlStyling = htmlStyling;
                                slider.Image = baseUrl + exactPath + FileName;

                                _context.TblSliders.Update(slider);
                            }
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        success = true;

                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblSliders.Max(x => (int?)x.SlideNo) ?? 0;
                            id = maxNumber + 1; ;
                            transaction.Rollback();
                            _context.ChangeTracker.Clear();
                        }
                        else
                        {
                            transaction.Rollback();
                            return Json(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(false);
                    }
                }
            }

            return Json(true);
        }


        public IActionResult GetSliders()
        {
            return Json(_context.TblSliders.AsNoTracking().OrderBy(o => o.SlideNo).ToList());
        }

        public IActionResult DelSliders(int id, string image)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //bool idCheck =
                    //    _context.TblObjects.Any(x => x.LocId.Equals(id)) ||
                    //    _context.TblIncidents.Any(x => x.LocId.Equals(id));


                    //bool idCheck =
                    //    _context.TblSliders.Any(x => x.SlideNo.Equals(id));

                    //if (idCheck)
                    //{
                    //    return Json("InUse");
                    //}

                    _context.TblSliders.Where(x => x.SlideNo == id).ExecuteDelete();
                    _context.SaveChanges();

                    if (!string.IsNullOrEmpty(image))
                    {
                        var webRootPath = _webRoot.WebRootPath;
                        var imageFilePath = Path.Combine(webRootPath, image);

                        if (System.IO.File.Exists(imageFilePath))
                        {
                            System.IO.File.Delete(imageFilePath);
                        }
                    }

                    transaction.Commit();

                    return Json(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }
        }

        #endregion


        #region CPM / Discount

        public IActionResult CpmEntry()
        {
            ViewBag.b2bRate = _context.TblSettings.Select(s => s.RateB2b).FirstOrDefault();

            ViewBag.ShowColumns = _context.TblSettings.Select(s => new {
                budgetFrom = s.ShowBudgetFrom,
                budgetTo = s.ShowBudgetTo,
                discount = s.ShowDiscount,
                cpm = s.ShowCpm,
                reach = s.ShowReach
            }).FirstOrDefault();

            return View();
        }


        public IActionResult SaveCpmEntry(int id, string budgetFrom, string budgetTo, 
                                          string discount, string reach)
        {
            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblCpms.Max(x => (int?)x.Id) ?? 0;
                            id = maxNoUnique + 1;


                            _context.TblCpms.Add(new TblCpm
                            {
                                Id = id,
                                BudgetFrom = budgetFrom,
                                BudgetTo = budgetTo,
                                Reach = reach,
                                Discount = discount,
                            });

                        }
                        else
                        {
                            var cpm = _context.TblCpms.Where(x => x.Id == id).FirstOrDefault();

                            if (cpm != null)
                            {

                                cpm.BudgetFrom = budgetFrom;
                                cpm.BudgetTo = budgetTo;
                                cpm.Reach = reach;
                                cpm.Discount = discount;

                                _context.TblCpms.Update(cpm);
                            }
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        success = true;

                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblCpms.Max(x => (int?)x.Id) ?? 0;
                            id = maxNumber + 1;
                            transaction.Rollback();
                            _context.ChangeTracker.Clear();
                        }
                        else
                        {
                            transaction.Rollback();
                            return Json(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(false);
                    }
                }
            }

            return Json(true);
        }

        public IActionResult GetCpmEntry()
        {
            return Json(_context.TblCpms.AsNoTracking().OrderBy(o => o.Id).ToList());
        }

        public IActionResult DelCpmEntry(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.TblCpms.Where(x => x.Id == id).ExecuteDelete();
                    _context.SaveChanges();
                    transaction.Commit();

                    return Json(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }
        }


        #endregion


        #region Screens

        public IActionResult Screen()
        {
            return View();
        }



        public string GetScreens(int stationid)
        {
            DataLogic dl = new DataLogic(_configuration);

            string qry = $@"Select  ScreenId, sc.StationId, ScreenName, ScreenSize, 
                            st.StationImage, XPosition, YPosition, XPosition1, YPosition1, sc.Rate, st.StationName ,
                            width,height from tblScreens sc Left Join tblStations st on st.stationid = sc.stationid 
                            where sc.Stationid ='{stationid}'";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }


        public IActionResult SaveScreen(int screenId, int stationId, string screenName, string screenSize,
                                         int playSecond, int playTime, string playPer, double rate)
        {
            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (screenId == 0)
                        {
                            var maxNoUnique = _context.TblScreens.Max(x => (int?)x.ScreenId) ?? 0;
                            screenId = maxNoUnique + 1;


                            _context.TblScreens.Add(new TblScreen
                            {
                                ScreenId = screenId,
                                StationId = stationId,
                                ScreenName = screenName,
                                ScreenSize = screenSize,
                                PlaySeconds = playSecond,
                                PlayTimes = playTime,
                                PlayPer = playPer,
                                Rate = rate,

                                Xposition = "0",
                                Yposition = "0",
                                Xposition1 = "0",
                                Yposition1 = "0",
                                Height = "0",
                                Width = "0",
                            });

                        }
                        else
                        {
                            var screen = _context.TblScreens.Where(x => x.ScreenId == screenId).FirstOrDefault();

                            if (screen != null)
                            {

                                screen.StationId = stationId;
                                screen.ScreenName = screenName;
                                screen.ScreenSize = screenSize;
                                screen.PlaySeconds = playSecond;
                                screen.PlayTimes = playTime;
                                screen.PlayPer = playPer;
                                screen.Rate = rate;


                                screen.Height = "0";
                                screen.Width = "0";

                                _context.TblScreens.Update(screen);
                            }
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        success = true;

                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblScreens.Max(x => (int?)x.ScreenId) ?? 0;
                            screenId = maxNumber + 1; ;
                            transaction.Rollback();
                            _context.ChangeTracker.Clear();
                        }
                        else
                        {
                            transaction.Rollback();
                            return Json(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(false);
                    }
                }
            }

            return Json(true);
        }

        public IActionResult GetScreen()
        {
            var screens = (from SC in _context.TblScreens
                         join ST in _context.TblStations on SC.StationId equals ST.StationId
                         select new
                         {
                             screenId = SC.ScreenId,
                             screenName = SC.ScreenName,
                             screenSize = SC.ScreenSize,
                             stationId = ST.StationId,
                             stationName = ST.StationName,
                             rate = SC.Rate ?? 0,
                             playSeconds = SC.PlaySeconds ?? 0,
                             playTimes = SC.PlayTimes ?? 0,
                             playPer = SC.PlayPer ?? "",

                             height = SC.Height,
                             width = SC.Width,
                         })
                         .AsNoTracking()
                         .ToList();
            
            return Json(screens);
        }

        public IActionResult DelScreen(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.TblScreens.Where(x => x.ScreenId == id).ExecuteDelete();
                    _context.SaveChanges();
                    transaction.Commit();

                    return Json(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }
        }

        #endregion


        #region Stations

        public IActionResult Station()
        {
            return View();
        }

        public IActionResult SaveStation(int id, string stationName,string address1, string address2, string lati,
                                      string longi, double rate, long dailyPassers, IFormFile stationImage)
        {
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/Station/";
            var upload = Path.Combine(webRootPath, exactPath);
            if (!Directory.Exists(upload))
            {
                Directory.CreateDirectory(upload);
            }

            var FileName = "";

            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblStations.Max(x => (int?)x.StationId) ?? 0;
                            id = maxNoUnique + 1;

                            if (stationImage != null)
                            {
                                if (stationImage.Length > 0)
                                {
                                    var extension = Path.GetExtension(stationImage.FileName);
                                    FileName = id + "-" + stationName + "-" + lati + "-" + longi + ".WebP";

                                    var img = new MagickImage(stationImage.OpenReadStream());
                                    img.Quality = (int)(img.Quality * 0.5);
                                    img.Format = MagickFormat.WebP;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        img.Write(filesStream);
                                    }
                                }
                            }

                            _context.TblStations.Add(new TblStation
                            {
                                StationId = id,
                                StationName = stationName,
                                Address1 = address1,
                                Address2 = address2,
                                Lat = lati,
                                Long = longi,
                                Rate = 0,
                                DailyPassers = dailyPassers,
                                StationImage = exactPath + FileName,

                            });

                        }
                        else
                        {
                            var station = _context.TblStations.Where(x => x.StationId == id).FirstOrDefault();

                            if (stationImage != null)
                            {
                                if (stationImage.Length > 0)
                                {
                                    var extension = Path.GetExtension(stationImage.FileName);
                                    FileName = id + "-" + stationName + "-" + lati + "-" + longi + ".WebP";

                                    var img = new MagickImage(stationImage.OpenReadStream());
                                    img.Quality = (int)(img.Quality * 0.5);
                                    img.Format = MagickFormat.WebP;


                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        img.Write(filesStream);
                                    }
                                }
                            }
                            else
                            {
                                FileName = station.StationImage;
                                exactPath = "";
                            }

                            if (station != null)
                            {

                                station.StationName = stationName;
                                station.Address1 = address1;
                                station.Address2 = address2;
                                station.Lat = lati;
                                station.Long = longi;
                                station.Rate = 0;
                                station.DailyPassers = dailyPassers;
                                station.StationImage = exactPath + FileName;

                                _context.TblStations.Update(station);
                            }
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        success = true;

                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblStations.Max(x => (int?)x.StationId) ?? 0;
                            id = maxNumber + 1; ;
                            transaction.Rollback();
                            _context.ChangeTracker.Clear();
                        }
                        else
                        {
                            transaction.Rollback();
                            return Json(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(false);
                    }
                }
            }

            return Json(true);
        }

        public IActionResult GetStation()
        {
            return Json(_context.TblStations.AsNoTracking().OrderBy(o => o.StationId).ToList());
        }

        public IActionResult DelStation(int id, string stationImage)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    //bool idCheck =
                    //    _context.TblObjects.Any(x => x.LocId.Equals(id)) ||
                    //    _context.TblIncidents.Any(x => x.LocId.Equals(id));


                    bool idCheck =
                        _context.TblScreens.Any(x => x.StationId.Equals(id));

                    if (idCheck)
                    {
                        return Json("InUse");
                    }

                    _context.TblStations.Where(x => x.StationId == id).ExecuteDelete();
                    _context.SaveChanges();

                    if (!string.IsNullOrEmpty(stationImage))
                    {
                        var webRootPath = _webRoot.WebRootPath;
                        var imageFilePath = Path.Combine(webRootPath, stationImage);

                        if (System.IO.File.Exists(imageFilePath))
                        {
                            System.IO.File.Delete(imageFilePath);
                        }
                    }

                    transaction.Commit();

                    return Json(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }
        }

        #endregion


        public class ScreenModel
        {
            public int Id { get; set; }
            public string X { get; set; }
            public string Y { get; set; }
            public string ScreenX { get; set; }
            public string ScreenY { get; set; }

            public string width { get; set; }
            public string height { get; set; }
        }

        #region Promotions

        public IActionResult Promotion()
        {
            return View();
        }


        public IActionResult SavePromotion(int id, DateTime? date, string message, string status, DateTime? lastSentDate,
                                           bool app, bool email, DateTime? scheduleTime)
        {
            if (scheduleTime != null) { status = "Schedule"; }
            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblPromotions.Max(x => (int?)x.Id) ?? 0;
                            id = maxNoUnique + 1;


                            _context.TblPromotions.Add(new TblPromotion
                            {
                                Id = id,
                                Date = date,
                                Message = message,
                                Status = status,
                                LastSentDate = lastSentDate,
                                App = app,
                                Email = email,
                                ScheduleTime = scheduleTime
                            });

                        }
                        else
                        {
                            var promo = _context.TblPromotions.Where(x => x.Id == id).FirstOrDefault();

                            if (promo != null)
                            {
                                promo.Message = message;
                                promo.Status = status;
                                promo.LastSentDate = lastSentDate;
                                promo.App = app;
                                promo.Email = email;
                                promo.ScheduleTime = scheduleTime;

                                _context.TblPromotions.Update(promo);
                            }
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        if (status.ToLower() == "sent")
                        {
                            _PushNotification.SendPushNotificationNew("", message, "Media Outdoor", "");
                        }

                        success = true;

                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblPromotions.Max(x => (int?)x.Id) ?? 0;
                            id = maxNumber + 1; ;
                            transaction.Rollback();
                            _context.ChangeTracker.Clear();
                        }
                        else
                        {
                            transaction.Rollback();
                            return Json(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(false);
                    }
                }
            }

            return Json(true);
        }

        public IActionResult GetPromotion()
        {
            return Json(_context.TblPromotions.AsNoTracking().ToList());
        }

        public IActionResult DelPromotion(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.TblPromotions.Where(x => x.Id == id).ExecuteDelete();
                    _context.SaveChanges();
                    transaction.Commit();

                    return Json(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }
        }




        #endregion


        #region PlaySettings

        public IActionResult PlaySettings()
        {
            return View();
        }

        public IActionResult SavePlaySettings(int id, int pSecond, int pTime, string pPer, string b2bRate, string b2cRate, bool appllyB2c)
        {
            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            _context.TblSettings.Add(new TblSetting
                            {
                                Id = 1,
                                PlaySeconds = pSecond,
                                PlayTimes = pTime,
                                PlayPer = pPer,
                                RateB2b = b2bRate,
                                RateB2c = b2cRate
                            });

                            if (appllyB2c == true)
                            {
                                var screensToUpdate = _context.TblStations.ToList();
                                screensToUpdate.ForEach(screen => screen.Rate = Convert.ToDouble(b2cRate));
                                _context.TblStations.UpdateRange(screensToUpdate);
                            }

                        }
                        else
                        {
                            var setting = _context.TblSettings.Where(x => x.Id == id).FirstOrDefault();

                            if (setting != null)
                            {
                                setting.PlaySeconds = pSecond;
                                setting.PlayTimes = pTime;
                                setting.PlayPer = pPer;

                                _context.TblSettings.Update(setting);
                            }

                            if (b2cRate != null || b2bRate != null)
                            {
                                var bRate = _context.TblSettings.Where(x => x.Id == id).ToList();

                                if (b2cRate != null)
                                {
                                    bRate.ForEach(rate => rate.RateB2c = b2cRate);
                                }

                                if (b2bRate != null)
                                {
                                    bRate.ForEach(rate => rate.RateB2b = b2bRate);
                                }

                                _context.TblSettings.UpdateRange(bRate);
                            }


                            if (appllyB2c == true)
                            {
                                var screensToUpdate = _context.TblStations.ToList();
                                screensToUpdate.ForEach(screen => screen.Rate = Convert.ToDouble(b2cRate));
                                _context.TblStations.UpdateRange(screensToUpdate);
                            }
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        success = true;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(false);
                    }
                }
            }

            return Json(true);
        }

        public IActionResult GetPlaySettings()
        {
            return Json(_context.TblSettings.AsNoTracking().FirstOrDefault());
        }

        public IActionResult DelPlaySettings(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.TblSettings.Where(x => x.Id == id).ExecuteDelete();
                    _context.SaveChanges();
                    transaction.Commit();

                    return Json(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(false);
                }
            }
        }

        #endregion


        #region Show CPM Columns


        public IActionResult SaveCPMColumns(bool BudgetFrom, bool BudgetTo, bool Discount, bool CPM, bool Reach)
        {
            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var setting = _context.TblSettings.Where(x => x.Id == 1).FirstOrDefault();

                        if (setting != null)
                        {
                            setting.ShowBudgetFrom = BudgetFrom;
                            setting.ShowBudgetTo = BudgetTo;
                            setting.ShowDiscount = Discount;
                            setting.ShowCpm = CPM;
                            setting.ShowReach = Reach;

                            _context.TblSettings.Update(setting);
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(false);
                    }
                }
            }

            return Json(true);
        }


        //public IActionResult GetCPMColumns()
        //{
        //    var data = _context.TblSettings.Select(x => new
        //    {
        //        BudgetFrom = x.ShowBudgetFrom,
        //        BudgetTo = x.ShowBudgetTo,
        //        Discount = x.ShowDiscount,
        //        CPM = x.ShowCpm,
        //        Reach = x.ShowReach
        //    }).AsNoTracking().FirstOrDefault();
        //    return Json(data);
        //}

        #endregion


        [HttpPost]
        public IActionResult SavePosition([FromBody] List<ScreenModel> positions)
        {
            try
            {
                foreach (var position in positions)
                {
                    // Retrieve the corresponding record from the database
                    var screen = _context.TblScreens.Where(x => x.ScreenId == position.Id).FirstOrDefault();

                    // Update the record with the new values
                    screen.Xposition = position.X;
                    screen.Yposition = position.Y;
                    screen.Xposition1 = position.ScreenX;
                    screen.Yposition1 = position.ScreenY;
                    screen.Width = position.width;
                    screen.Height = position.height;
                    // Save changes to the database
                    _context.SaveChanges();
                }

                return Json(true); // Return a success response
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return Json(false);
            }
        }

    }
}
