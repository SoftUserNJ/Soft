using Microsoft.AspNetCore.Hosting;
using CityTech.Models;
using CityTech.Sevices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace CityTech.Controllers
{
    public class ArticleController : Controller
    {
        private readonly CityTechContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webRoot;
        private readonly ILog _log;

        public ArticleController(CityTechContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment hostingEnvironment, ILog log)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webRoot = hostingEnvironment;
            _log = log;
        }

        private bool IsPrimaryKeyViolation(DbUpdateException ex)
        {
            // Check if the exception indicates a primary key violation
            // This can vary depending on the database provider you're using
            // You may need to adapt this based on your database provider's error codes or messages
            return ex.InnerException is SqlException sqlException && sqlException.Number == 2627;
        }
        [AllowPage]
        public IActionResult Articles()
        {
            return View();
        }

        #region Article
        public string GetArticle()
        {
            DataLogic dl = new DataLogic(_configuration);
            string qry = @"SELECT A.ARTICLENO AS ID, A.NAME AS NAME, G.GROUPDESCRIPTION AS GROUPDESCRIPTION, G.ID AS GROUPDESCRIPTIONID, A.UOM AS UOM, A.IMGPATH AS IMG 
                           FROM TBLARTICLES A INNER JOIN tblarticlegroup G ON G.ID = A.GROUPID";

            var dt = dl.LoadData(qry);

            var articleMaxN = _context.TblArticles.Max(x => (int?)x.ArticleNo) ?? 0;
            var narticleMaxN = articleMaxN + 1;

            var groupMaxN = _context.TblArticleGroups.Max(x => (int?)x.Id) ?? 0;
            var ngroupMaxN = groupMaxN + 1;


            var data = new
            {
                articleMaxNumber = narticleMaxN,
                groupMaxNumber = ngroupMaxN,
                list = dt
            };

            return JsonConvert.SerializeObject(data);
        }
        public IActionResult SaveArticle(int id, string name, int groupId, string uom, IFormFile Image, string status, DateTime activityLogDateTime)
        {
            var webRootPath = _webRoot.WebRootPath;
            var exactPath = "Images/Artical/";
            var upload = Path.Combine(webRootPath, exactPath);
            var FileName = "";

            bool success = false;

            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (status == null)
                        {
                            int maxNumber = _context.TblArticles.Max(x => (int?)x.ArticleNo) ?? 0;
                            id = maxNumber + 1;

                            if (Image != null)
                            {
                                if (Image.Length > 0)
                                {
                                    var extension = Path.GetExtension(Image.FileName);
                                    FileName = id + "-" + name + "-" + uom + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        Image.CopyTo(filesStream);
                                    }
                                }
                            }

                            _context.TblArticles.Add(new TblArticle
                            {
                                ArticleNo = id,
                                Name = name,
                                GroupId = groupId,
                                Uom = uom,
                                ImgPath = exactPath + FileName
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Article No."+id+" Name: "+ name + " Added", 0, "", "");


                        }
                        else
                        {
                            var articleUpdate = _context.TblArticles.Where(x => x.ArticleNo == id).FirstOrDefault();

                            if (Image != null)
                            {
                                if (Image.Length > 0)
                                {
                                    var extension = Path.GetExtension(Image.FileName);
                                    FileName = id + "-" + name + "-" + uom + extension;

                                    using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                                    {
                                        Image.CopyTo(filesStream);
                                    }
                                }
                            }
                            else
                            {
                                FileName = articleUpdate.ImgPath;
                                exactPath = "";
                            }

                            articleUpdate.Name = name;
                            articleUpdate.GroupId = groupId;
                            articleUpdate.Uom = uom;
                            articleUpdate.ImgPath = exactPath + FileName;
                            _context.TblArticles.Update(articleUpdate);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Article No."+id+" Name: "+ name + " Edited", 0, "", "");


                        }

                        _context.SaveChanges();
                        transaction.Commit(); 
                        success = true;
                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblArticles.Max(x => (int?)x.ArticleNo) ?? 0;
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

        public IActionResult DelArticle(int id, string imgName, DateTime activityLogDateTime)
        {
            string name = "";

            var del = _context.TblArticles.Where(d => d.ArticleNo.Equals(id)).FirstOrDefault();
            name = del.Name;

            _context.Remove(del);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Article No."+id+" Name: "+ name + " Deleted", 0, "", "");


            if (!string.IsNullOrEmpty(imgName))
            {
                var webRootPath = _webRoot.WebRootPath;
                var imageFilePath = Path.Combine(webRootPath, imgName);

                if (System.IO.File.Exists(imageFilePath))
                {
                    System.IO.File.Delete(imageFilePath);
                }
            }

            return Json(true);
        }

        #endregion

        #region UOM
        [AllowPage]
        public IActionResult UOM()
        {
            return View();
        }

        public IActionResult GetUom()
        {
            return Json(_context.TblUoms.ToList());
        }

        public IActionResult SaveUom(int id, string uom, DateTime activityLogDateTime)
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
                            var maxNoUnique = _context.TblUoms.Max(x => (int?)x.Id) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblUoms.Add(new TblUom
                            {
                                Id = id,
                                Uom = uom,
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Article UOM No."+id+" Name: "+ uom + "  Added", 0, "", "");

                        }
                        else
                        {
                            var uomUpdate = _context.TblUoms.Where(x => x.Id == id).FirstOrDefault();
                            uomUpdate.Uom = uom;
                            _context.TblUoms.Update(uomUpdate);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Article UOM No."+id+" Name: "+ uom + "  Edited", 0, "", "");

                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        success = true;
                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblUoms.Max(x => (int?)x.Id) ?? 0;
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

        public IActionResult DelUom(int id, DateTime activityLogDateTime)
        {
            string uom = "";
            var delUom = _context.TblUoms.Where(d => d.Id.Equals(id)).FirstOrDefault();
            uom = delUom.Uom;
            _context.Remove(delUom);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Article UOM No."+id+" Name: "+ uom + "  Deleted", 0, "", "");

            return Json(true);
        }

        #endregion

        #region Groups
        [AllowPage]
        public IActionResult ArticleGroup()
        {
            return View();
        }

        public IActionResult GetGroup()
        {
            int maxNumber = _context.TblArticleGroups.Max(x => (int?)x.Id) ?? 0;
            int maxN = maxNumber + 1;
            return Json(_context.TblArticleGroups.Select(x => new { id = x.Id, groupDescription = x.GroupDescription, maxno = maxN }).ToList());
            //return Json(_context.TblArticleGroups.ToList());
        }

        public IActionResult SaveGroup(int id, string description, string status, DateTime activityLogDateTime)
        {
            bool success = false;
            while (!success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (status == null)
                        {
                            var maxNoUnique = _context.TblArticleGroups.Max(x => (int?)x.Id) ?? 0;
                            id = maxNoUnique + 1;

                            _context.TblArticleGroups.Add(new TblArticleGroup
                            {
                                Id = id,
                                GroupDescription = description
                            });
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Article Group No."+id+" Name: "+ description + "  Added", 0, "", "");

                        }
                        else
                        {
                            var grpUpdate = _context.TblArticleGroups.Where(x => x.Id == id).FirstOrDefault();
                            grpUpdate.GroupDescription = description;
                            _context.TblArticleGroups.Update(grpUpdate);
                            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Article Group No."+id+" Name: "+ description + "  Edited", 0, "", "");

                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        success = true;
                    }
                    catch (DbUpdateException ex)
                    {
                        if (IsPrimaryKeyViolation(ex))
                        {
                            int maxNumber = _context.TblArticleGroups.Max(x => (int?)x.Id) ?? 0;
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

        public IActionResult DelGroup(int id, DateTime activityLogDateTime)
        {
            bool idCheck =
                        _context.TblArticles.Any(x => x.GroupId.Equals(id));

            if (idCheck)
            {
                return Json("Already In Use");
            }

            string Group = "";
            var delGrp = _context.TblArticleGroups.Where(d => d.Id.Equals(id)).FirstOrDefault();
            Group = delGrp.GroupDescription;
            _context.Remove(delGrp);
            _context.SaveChanges();
            _log.LogEntry(activityLogDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), "Article Group No." + id + " Name: " + Group + "  Deleted", 0, "", ""); ;

            return Json(true);
        }

        #endregion

    }
}


