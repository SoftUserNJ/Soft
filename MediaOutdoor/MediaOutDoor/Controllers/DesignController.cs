using CityTechWEBAPI;
using MediaOutdoor.Models;
using MediaOutDoor.Models;
using MediaOutDoor.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Transactions;

namespace MediaOutDoor.Controllers
{
    public class DesignController : Controller
    {
        private readonly MediaOutdoorContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public DesignController(MediaOutdoorContext dbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public IActionResult UploadYourDesign()
        {
            return View();
        }

        public IActionResult DesignMV()
        {
            return View();
        }

        public class SaveDesignRequest
        {
            public string VisitorId { get; set; }
            public List<ScreenData> ScreenDataArray { get; set; }
            public List<SlotDataViewModel> SlotDataArray { get; set; }
        }


        public class ScreenData
        {
            public int ScreenId { get; set; }
            public string Text1 { get; set; }
            public string Text2 { get; set; }
            public string Text3 { get; set; }
            public string Color1 { get; set; }
            public string Color2 { get; set; }
            public string Color3 { get; set; }
            public string Font1 { get; set; }
            public string Font2 { get; set; }
            public string Font3 { get; set; }
            public string LeftValue1 { get; set; }
            public string TopValue1 { get; set; }
            public string LeftValue2 { get; set; }
            public string TopValue2 { get; set; }
            public string LeftValue3 { get; set; }
            public string TopValue3 { get; set; }
            public string FontSize1 { get; set; }
            public string FontSize2 { get; set; }
            public string FontSize3 { get; set; }
            public string ImageDataURI { get; set; }
        }


        public class SlotDataViewModel
        {
            public string SelectedDate { get; set; }
            public TimeSpan SlotFrom { get; set; }
            public TimeSpan SlotTo { get; set; }
            public int NoOfSlots { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> SaveDesign([FromBody] SaveDesignRequest request, [FromQuery] string visitorId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var screenDataList = request.ScreenDataArray;
                    var slotDataList = request.SlotDataArray;

                    foreach (var screenData in screenDataList)
                    {
                        var screenId = screenData.ScreenId;
                        var screen = await _dbContext.TblOrders.FirstOrDefaultAsync(u => u.VisitorId == visitorId && u.ScreenId == screenId);
                        if (screen != null)
                        {
                            screen.Text1 = screenData.Text1;
                            screen.Text2 = screenData.Text2;
                            screen.Text3 = screenData.Text3;
                            screen.Text1Color = screenData.Color1;
                            screen.Text2Color = screenData.Color2;
                            screen.Text3Color = screenData.Color3;
                            screen.Text1Font = screenData.Font1;
                            screen.Text2Font = screenData.Font2;
                            screen.Text3Font = screenData.Font3;
                            screen.Text1LeftPosition = screenData.LeftValue1;
                            screen.Text2LeftPosition = screenData.LeftValue2;
                            screen.Text3LeftPosition = screenData.LeftValue3;
                            screen.Text1TopPosition = screenData.TopValue1;
                            screen.Text2TopPosition = screenData.TopValue2;
                            screen.Text3TopPosition = screenData.TopValue3;
                            screen.Text1Size = screenData.FontSize1;
                            screen.Text2Size = screenData.FontSize2;
                            screen.Text3Size = screenData.FontSize3;

                            //var extension = Path.GetExtension(screenData.ImageDataURI);
                            var fileName = screen.VisitorId + "-" + screen.OrderId + ".jpg";

                            var targetImagePath = Path.Combine("D:\\wwwroot\\MediaOutdoorBackend\\wwwroot\\Images\\Orders", fileName);

                            // Validate and clean the base64 string (remove data URI prefix if present)
                            var base64Data = CleanBase64String(screenData.ImageDataURI);

                            // Attempt to decode the cleaned base64 string
                            var imageDataBytes = Convert.FromBase64String(base64Data);

                            using (var stream = new FileStream(targetImagePath, FileMode.Create))
                            {
                                await stream.WriteAsync(imageDataBytes, 0, imageDataBytes.Length);
                            }

                            screen.OrderImage = $"Images/Orders/{fileName}";

                            _dbContext.TblOrders.Update(screen);
                        }
                    }

                    var existingSchedules = _dbContext.TblOrderSchedules.Where(schedule => schedule.VisitorId == visitorId).ToList();
                    if (existingSchedules.Any())
                    {
                        _dbContext.TblOrderSchedules.RemoveRange(existingSchedules);
                    }


                    foreach (var slotData in slotDataList)
                    {
                        var schedule = new TblOrderSchedule
                        {
                            VisitorId = visitorId,
                            PlayDate = DateTime.Parse(slotData.SelectedDate),
                            SlotFrom = slotData.SlotFrom,
                            SlotTo = slotData.SlotTo,
                            NoOfSlots = slotData.NoOfSlots
                        };

                        _dbContext.TblOrderSchedules.Add(schedule);
                    }

                    await _dbContext.SaveChangesAsync();
                    scope.Complete();
                    return Ok("Saved");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, e.g., log or return an error response
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }


        }

        [HttpPost]
        public async Task<IActionResult> EditDesign([FromBody] SaveDesignRequest request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var csid = new SessionData(_httpContextAccessor).GetData();
                    foreach (var screenData in request.ScreenDataArray)
                    {
                        var screenId = screenData.ScreenId;
                        var screen = await _dbContext.TblOrders.FirstOrDefaultAsync(u => u.CustomerId == csid.UserId && u.ScreenId == screenId);
                        if (screen != null)
                        {
                            screen.Text1 = screenData.Text1;
                            screen.Text2 = screenData.Text2;
                            screen.Text3 = screenData.Text3;
                            screen.Text1Color = screenData.Color1;
                            screen.Text2Color = screenData.Color2;
                            screen.Text3Color = screenData.Color3;
                            screen.Text1Font = screenData.Font1;
                            screen.Text2Font = screenData.Font2;
                            screen.Text3Font = screenData.Font3;
                            screen.Text1LeftPosition = screenData.LeftValue1;
                            screen.Text2LeftPosition = screenData.LeftValue2;
                            screen.Text3LeftPosition = screenData.LeftValue3;
                            screen.Text1TopPosition = screenData.TopValue1;
                            screen.Text2TopPosition = screenData.TopValue2;
                            screen.Text3TopPosition = screenData.TopValue3;
                            screen.Text1Size = screenData.FontSize1;
                            screen.Text2Size = screenData.FontSize2;
                            screen.Text3Size = screenData.FontSize3;

                            if (screenData.ImageDataURI != null)
                            {
                                if (screenData.ImageDataURI.StartsWith("data:image/")) // Check if it's a base64 image
                                {
                                    var fileName = screen.VisitorId + "-" + screen.OrderId + ".jpg";
                                    var targetImagePath = Path.Combine("D:\\wwwroot\\MediaOutdoorBackend\\wwwroot\\Images\\Orders", fileName);

                                    // Validate and clean the base64 string (remove data URI prefix if present)
                                    var base64Data = CleanBase64String(screenData.ImageDataURI);

                                    // Attempt to decode the cleaned base64 string
                                    var imageDataBytes = Convert.FromBase64String(base64Data);

                                    using (var stream = new FileStream(targetImagePath, FileMode.Create))
                                    {
                                        await stream.WriteAsync(imageDataBytes, 0, imageDataBytes.Length);
                                    }

                                    screen.OrderImage = $"Images/Orders/{fileName}";
                                }
                            }

                            _dbContext.TblOrders.Update(screen);
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                    scope.Complete(); // Commit the transaction

                    return Ok("Saved");
                }
                catch (Exception ex)
                {
                    // Handle the exception, e.g., log or return an error response
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
                finally
                {
                    scope.Dispose();
                }
            }
        }


        private string CleanBase64String(string base64Data)
        {
            const string dataUriPrefixPng = "data:image/png;base64,";
            const string dataUriPrefixJpeg = "data:image/jpeg;base64,";
            const string dataUriPrefixJpg = "data:image/jpg;base64,";

            if (base64Data.StartsWith(dataUriPrefixPng))
            {
                return base64Data.Substring(dataUriPrefixPng.Length);
            }
            else if (base64Data.StartsWith(dataUriPrefixJpeg))
            {
                return base64Data.Substring(dataUriPrefixJpeg.Length);
            }
            else if (base64Data.StartsWith(dataUriPrefixJpg))
            {
                return base64Data.Substring(dataUriPrefixJpg.Length);
            }

            return base64Data;
        }



        public string GetSlots()
        {
            DataLogic dl = new DataLogic(_configuration);

            string qry = "Select * from tblslots";
            var dt = dl.LoadData(qry);

            return JsonConvert.SerializeObject(dt);
        }
    }
}
