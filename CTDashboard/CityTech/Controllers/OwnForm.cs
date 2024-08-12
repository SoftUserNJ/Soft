using CityTech.Models;
using CityTech.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using CityTech.Models.ViewModel;
//using ImageMagick;
namespace CityTech.Controllers
{

    public class OwnForm : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CityTechContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public OwnForm(ILogger<HomeController> logger, CityTechContext context, IHttpContextAccessor httpContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContext;
            _hostingEnvironment = webHostEnvironment;
            _configuration = configuration;
        }


        public IActionResult OwnFormsList()
        {
            return View();
        }

        public string GetOwnFormsList()
        {
            DataLogic dl = new DataLogic(_configuration);
            string qry = @"Select * from TblOwnForm";
            var dt = dl.LoadData(qry);
            return JsonConvert.SerializeObject(dt);

        }
        public bool DeleteUsedArticle(int formId)
        {
            try
            {
                // Find the form by its ID and remove it
                var form = _context.Tblownforms.FirstOrDefault(f => f.Formid == formId);

                if (form != null)
                {
                    _context.Tblownforms.Remove(form);
                    _context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    public IActionResult Camera()

        {
            return View();
        }

    public IActionResult Builder(int? FormId)
        {
            string FormData = "";
            string FormName = "";
            int Customerid=0;
            if (FormId != null)
            {
                DataLogic dl = new DataLogic(_configuration);
                String Qry = @" select *  from tblownform where Formid='" + FormId + "'";
                var dt1 = dl.LoadData(Qry);

                if (dt1.Rows.Count > 0)
                {
                    FormData = dt1.Rows[0]["FormData"].ToString();
                    FormName = dt1.Rows[0]["FormName"].ToString();
                    Customerid = Convert.ToInt32( dt1.Rows[0]["Customerid"]);
                }
            }
            else { FormId = 0; }
            var FormBuilderModel = new FormBuilder
            {
                FormId = FormId,
                FormData = FormData,
                FormName= FormName ,
                Customerid= Customerid
            };

            return View("Builder", FormBuilderModel);
        }




        //public IActionResult Pdf(int FormId , int  IncidentNo)
        //{
        //    string FormData = "";
        //    string FormName = "";
        //    int Customerid = 0;
       
        //        DataLogic dl = new DataLogic(_configuration);
        //        String Qry = @" select *  from tblIncownform where Formid='" + FormId + "' and  IncidentNo ='"+ IncidentNo + "'";
        //        var dt1 = dl.LoadData(Qry);

        //        if (dt1.Rows.Count > 0)
        //        {
                   
        //            FormName = dt1.Rows[0]["FormName"].ToString();



        //        string originalFormData = dt1.Rows[0]["FormData"].ToString();
        //        string modifiedFormData = originalFormData.Replace("table-responsive", "mobileview");
        //        FormData = modifiedFormData;


        //    }
         
        
        //    var FormBuilderModel = new FormBuilder
        //    {
        //        FormId = FormId,
        //        FormData = FormData,
        //        FormName = FormName,
        //        Customerid = Customerid
        //    };

        //    return View("Pdf", FormBuilderModel);
        //}



        public IActionResult Pdf(int FormId, int IncidentNo, string  WorkAddress)
        {

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ownform");
            string fileName = $"FormData_{FormId}_{IncidentNo}.html";
            string fullPath = Path.Combine(folderPath, fileName);
            string FormData = null;

            if (System.IO.File.Exists(fullPath))
            {
                string FormDataFromFile = System.IO.File.ReadAllText(fullPath);
                
                FormData = FormDataFromFile.Replace("table-responsive", "mobileview");

            }
            else
            {

                FormData = "<p>Form data not found.</p>";
            }


            var viewModel = new ViewForm
            {
                FormId = FormId,
                FormName = "",
                IncidentNo = IncidentNo,
                FormData = FormData,
                Location= WorkAddress
            };


            return View(viewModel);
        }




        public IActionResult ViewForm(int FormId, string FormName, int IncidentNo)
        {
       
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ownform");
            string fileName = $"FormData_{FormId}_{IncidentNo}.html";
            string fullPath = Path.Combine(folderPath, fileName);
            string FormData = null;

            if (System.IO.File.Exists(fullPath))
            {
                
                FormData = System.IO.File.ReadAllText(fullPath);
            }
            else
            {
               
                DataLogic dl = new DataLogic(_configuration);
                String Qry = @"SELECT * FROM tblincownform WHERE Formid='" + FormId + "' AND Incidentno='" + IncidentNo + "'";
                var dt1 = dl.LoadData(Qry);

                if (dt1.Rows.Count > 0)
                {
                    string originalFormData = dt1.Rows[0]["FormData"].ToString();
                    string modifiedFormData = originalFormData.Replace("mobileview", "table-responsive");
                    FormData = modifiedFormData;
                }
                else
                {
                    
                    FormData = "<p>Form data not found.</p>";
                }
            }

            
            var viewModel = new ViewForm
            {
                FormId = FormId,
                FormName = FormName,
                IncidentNo = IncidentNo,
                FormData = FormData
            };

          
            return View(viewModel);
        }



        public class ViewFormSave
        {
            public string Formid { get; set; }
            public string Incidentno { get; set; }
            public string FormData { get; set; }
        }


        //public IActionResult SaveViewForm(string Formid, string Incidentno, string FormData)
        //{


        [HttpPost]
        public IActionResult SaveViewForm([FromBody] ViewFormSave model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var Incownforms = _context.TblIncownforms.FirstOrDefault(x => x.Formid == Convert.ToInt32(model.Formid) && x.IncidentNo == Convert.ToInt32(model.Incidentno));
                    if (Incownforms != null)
                    {
                        Incownforms.Count = (Incownforms.Count ?? 0) + 1;
                        _context.TblIncownforms.Update(Incownforms);
                        _context.SaveChanges();

                        // Save FormData to HTML file
                        SaveFormDataToHtml(model.Formid, model.Incidentno, model.FormData);

                        transaction.Commit();
                        return Json(true);
                    }
                    else
                    {
                        return Json("No record found for the provided FORMID and INCIDENTNO.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = ex.Message });
                }
            }
        }

        private void SaveFormDataToHtml(string formId, string incidentNo, string formData)
        {
            // Construct the file name
            string fileName = $"FormData_{formId}_{incidentNo}.html";
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ownform");
            string fullPath = Path.Combine(folderPath, fileName);

            // Ensure the directory exists
            Directory.CreateDirectory(folderPath); // This method is safe to call if the directory already exists

            // Write the FormData to the HTML file
            System.IO.File.WriteAllText(fullPath, formData);
        }


        public IActionResult GetCompanyInfo()
        {
            var tblcompany = _context.TblCompnays.Select(x => new {
                id = x.Id,
                name = x.Name ?? "",
                email = x.Email ?? "",
                telephone = x.Telephone ?? "",
                city = x.City ?? "",
                postalcode = x.PostalCode ?? "",
                address1 = x.Address1 ?? "",
                address2 = x.Address2 ?? "",
                imgpath = x.ImgPath ?? "",

            }).OrderBy(o => o.name).ToList();
            return Json(tblcompany);
        }

 


        public IActionResult SaveForm(string FORMID, string FORMNAME, string FORMDATA , bool Mandatory, bool AutoAttach , int Customerid)
        {
           

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var SessionData = new SessionData(_httpContextAccessor).GetData();
                    var DashBoard = _context.Tblownforms.FirstOrDefault(x => x.Formid == Convert.ToInt32( FORMID));

                    if (DashBoard == null)
                    {
                        int Newid = 0;
                        int? MaxNewid = _context.Tblownforms.Max(i => (int?)i.Formid);
                        if (MaxNewid.HasValue)
                        {
                            Newid = MaxNewid.Value + 1;
                        }
                        else
                        {

                            Newid = 1;
                        }
                        _context.Tblownforms.Add(new Tblownform
                        {
                            Formid = Newid,
                            Formname = FORMNAME, 
                            Formdata= FORMDATA ,
                            Mandatory= Mandatory,
                            AutoAttach= AutoAttach,
                            Customerid= Customerid

                            //TabContent = TabContent,

                        });
                    }
                    else
                    {


                        DashBoard.Formname = FORMNAME;
                        DashBoard.Formdata = FORMDATA;
                        DashBoard.Mandatory = Mandatory;
                        DashBoard.AutoAttach = AutoAttach;
                        DashBoard.Customerid = Customerid;
                        _context.Tblownforms.Update(DashBoard);
                    }
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














    }
}