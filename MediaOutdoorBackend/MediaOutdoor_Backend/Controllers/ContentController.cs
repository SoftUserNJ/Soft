using ImageMagick;
using MediaOutdoor_Backend.Models;
using MediaOutdoor_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace MediaOutdoor_Backend.Controllers
{
    public class ContentController : Controller
    {

        private readonly MediaOutdoorContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webRoot;


        public ContentController(MediaOutdoorContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webRoot)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webRoot = webRoot;
        }

        private bool IsPrimaryKeyViolation(DbUpdateException ex)
        {
            // Check if the exception indicates a primary key violation
            // This can vary depending on the database provider you're using
            // You may need to adapt this based on your database provider's error codes or messages
            return ex.InnerException is SqlException sqlException && sqlException.Number == 2627;
        }


        #region Content Category
        public IActionResult ContentCategory()
        {
            return View();
        }

        public JsonResult GetContentCategory()
        {
            return Json(_context.TblContentCats.AsNoTracking().ToList());
        }

        public IActionResult SaveContentCategory(int id, string category, IFormFile icon)
        {
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/";
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/Content/";
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
                            var maxNoUnique = _context.TblContentCats.Max(x => (int?)x.CatId) ?? 0;
                            id = maxNoUnique + 1;

                            if (icon != null)
                            {
                                if (icon.Length > 0)
                                {
                                    var extension = Path.GetExtension(icon.FileName);
                                    FileName = id + "-Icon-" + category + ".WebP";

                                    var img = new MagickImage(icon.OpenReadStream());
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
                                FileName = "";
                                exactPath = "";
                                url = "";
                            }

                            _context.TblContentCats.Add(new TblContentCat
                            {
                                CatId = id,
                                Category = category,
                                Type = "Private",
                                Icon = url + exactPath + FileName,
                            });

                        }
                        else
                        {
                            var Update = _context.TblContentCats.Where(x => x.CatId == id).FirstOrDefault();

                            if (icon != null)
                            {
                                if (icon.Length > 0)
                                {
                                    var extension = Path.GetExtension(icon.FileName);
                                    FileName = id + "-Icon-" + category + ".WebP";

                                    var img = new MagickImage(icon.OpenReadStream());
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
                                FileName = Update.Icon;
                                exactPath = "";
                                url = "";
                            }

                            if (Update != null)
                            {

                                Update.Category = category;
                                Update.Type = "Private";
                                Update.Icon = url + exactPath + FileName;
                                _context.TblContentCats.Update(Update);
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
                            int maxNumber = _context.TblContentCats.Max(x => (int?)x.CatId) ?? 0;
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

        public IActionResult DelContentCategory(int id)
        {

            bool idCheck = _context.TblContentDetails.Any(x => x.CatId.Equals(id));
            if (idCheck)
            {
                return Json("Already In Use");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.TblContentCats.Where(x => x.CatId == id).ExecuteDelete();
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

        #region Content Detail

        public IActionResult ContentDetail()
        {
            return View();
        }

        public JsonResult GetContentDetail()
        {

            var dbuser = (from D in _context.TblContentDetails
                          join C in _context.TblContentCats on D.CatId equals C.CatId
                          select new
                          {
                              id = D.ContentId,
                              img1 = D.SingleImage ?? "",
                              img2 = D.DoubleImage ?? "",
                              img3 = D.TrippleImage ?? "",
                              category = C.Category ?? "",
                              categoryId = C.CatId,

                          }).AsNoTracking().ToList();


            return Json(dbuser);
        }


        public IActionResult SaveContentDetail(int id, int catId, IFormFile userImg1, IFormFile userImg2, IFormFile userImg3)
        {
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/";
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/Content/";
            var upload = Path.Combine(webRootPath, exactPath);
            var FileName1 = "";
            var FileName2 = "";
            var FileName3 = "";

            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id == 0)
                        {
                            var maxNoUnique = _context.TblContentDetails.Max(x => (int?)x.ContentId) ?? 0;
                            id = maxNoUnique + 1;

                            if (userImg1 != null && userImg2 != null && userImg3 != null)
                            {
                                if (userImg1.Length > 0)
                                {
                                    var extension = Path.GetExtension(userImg1.FileName);
                                    FileName1 = id + "-"+ "Single-" + catId + ".WebP";

                                    var img = new MagickImage(userImg1.OpenReadStream());
                                    img.Quality = (int)(img.Quality * 0.5);
                                    img.Format = MagickFormat.WebP;


                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName1), FileMode.Create))
                                    {
                                        img.Write(filesStream);
                                    }
                                }


                                if (userImg2.Length > 0)
                                {
                                    var extension = Path.GetExtension(userImg2.FileName);
                                    FileName2 = id + "-" + "Double-" + catId + ".WebP";

                                    var img = new MagickImage(userImg2.OpenReadStream());
                                    img.Quality = (int)(img.Quality * 0.5);
                                    img.Format = MagickFormat.WebP;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName2), FileMode.Create))
                                    {
                                        img.Write(filesStream);
                                    }
                                }


                                if (userImg3.Length > 0)
                                {
                                    var extension = Path.GetExtension(userImg3.FileName);
                                    FileName3 = id + "-" + "Triple-" + catId + ".WebP";

                                    var img = new MagickImage(userImg3.OpenReadStream());
                                    img.Quality = (int)(img.Quality * 0.5);
                                    img.Format = MagickFormat.WebP;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName3), FileMode.Create))
                                    {
                                        img.Write(filesStream);
                                    }
                                }
                            }
                            else
                            {
                                FileName1 = "";
                                FileName2 = "";
                                FileName3 = "";
                                exactPath = "";
                                url = "";
                            }

                            _context.TblContentDetails.Add(new TblContentDetail
                            {
                                ContentId = id,
                                SingleImage = url + exactPath + FileName1,
                                DoubleImage = url + exactPath + FileName2,
                                TrippleImage = url + exactPath + FileName3,
                                CatId = catId,
                            });

                        }
                        else
                        {
                            var cont = _context.TblContentDetails.Where(x => x.ContentId == id).FirstOrDefault();

                            if (userImg1 != null)
                            {
                                if (userImg1.Length > 0)
                                {
                                    var extension = Path.GetExtension(userImg1.FileName);
                                    FileName1 = id + "-" + "Single-" + catId + ".WebP";

                                    var img = new MagickImage(userImg1.OpenReadStream());
                                    img.Quality = (int)(img.Quality * 0.5);
                                    img.Format = MagickFormat.WebP;


                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName1), FileMode.Create))
                                    {
                                        img.Write(filesStream);
                                    }
                                }

                                FileName1 = url + exactPath + FileName1;
                            }
                            else
                            {
                                FileName1 = cont.SingleImage;
                            }

                            if (userImg2 != null)
                            {
                                if (userImg2.Length > 0)
                                {
                                    var extension = Path.GetExtension(userImg2.FileName);
                                    FileName2 = id + "-" + "Double-" + catId + ".WebP";

                                    var img = new MagickImage(userImg2.OpenReadStream());
                                    img.Quality = (int)(img.Quality * 0.5);
                                    img.Format = MagickFormat.WebP;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName2), FileMode.Create))
                                    {
                                        img.Write(filesStream);
                                    }
                                }

                                FileName2 = url + exactPath + FileName2;
                            }
                            else
                            {
                                FileName2 = cont.DoubleImage;
                            }

                            if (userImg3 != null)
                            {
                                if (userImg3.Length > 0)
                                {
                                    var extension = Path.GetExtension(userImg3.FileName);
                                    FileName3 = id + "-" + "Triple-" + catId + ".WebP";

                                    var img = new MagickImage(userImg3.OpenReadStream());
                                    img.Quality = (int)(img.Quality * 0.5);
                                    img.Format = MagickFormat.WebP;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName3), FileMode.Create))
                                    {
                                        img.Write(filesStream);
                                    }
                                }

                                FileName3 = url + exactPath + FileName3;
                            }
                            else
                            {
                                FileName3 = cont.TrippleImage;
                            }


                            if (cont != null)
                            {

                                cont.SingleImage = FileName1;
                                cont.DoubleImage = FileName2;
                                cont.TrippleImage = FileName3;
                                cont.CatId = catId;

                                _context.TblContentDetails.Update(cont);
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
                            int maxNumber = _context.TblContentDetails.Max(x => (int?)x.ContentId) ?? 0;
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

        public IActionResult DelContentDetail(int id, string profilePic)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.TblContentDetails.Where(x => x.ContentId == id).ExecuteDelete();
                    _context.SaveChanges();

                    if (!string.IsNullOrEmpty(profilePic))
                    {
                        var webRootPath = _webRoot.WebRootPath;
                        var imageFilePath = Path.Combine(webRootPath, profilePic);

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
    }
}
