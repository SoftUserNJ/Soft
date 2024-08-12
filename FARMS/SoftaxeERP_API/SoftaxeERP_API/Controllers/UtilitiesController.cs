using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.Services;
using SoftaxeERP_API.VM;

namespace SoftaxeERP_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {

        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        private readonly IWebHostEnvironment _hostingEnvironment;

        readonly AuthVM auth = new();
        public UtilitiesController(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
            _hostingEnvironment = hostingEnvironment;
        }

        #region USER LOG

        [HttpGet("UserLogStatus")]
        public IActionResult UserLogStatus(DateTime fromDate, DateTime toDate, string locId)
        {
            string qry = $@"SELECT U.ID AS USERID, L.ID, CONVERT(VARCHAR(8),L.VDATE,108) AS TIME,CONVERT(VARCHAR(10),L.VDATE,103) AS DATE, U.USERNAME,L.locid as Loc, L.VTYPE AS TYPE,ISNULL(L.VCHNO,0) AS VCHNO, 
            ISNULL(CONVERT(VARCHAR(10),L.VHRDATE,103),'') AS VCHDATE, L.REMRAKS AS REMARKS, ISNULL(PURCHASERATE,0) AS PURCHASERATE, ISNULL(MINRATE,0) AS MINRATE, ISNULL(MAXRATE,0) AS MAXRATE,ISNULL(AMOUNT,0) AS AMOUNT
            FROM TBLLOG L
            INNER JOIN USERS U ON L.UID = U.ID AND U.CMP_ID = L.CMP_ID AND L.LOCID = U.LOCID
            WHERE L.CMP_ID = {auth.CmpId} AND L.LOCID LIKE '{locId}' AND L.FINID = {auth.FinId} AND CONVERT(VARCHAR(10),VDATE,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' ORDER BY L.ID DESC";

            string result = JsonConvert.SerializeObject(_dataLogic.LoadData(qry));
            return Ok(result);
        }

        [HttpGet("GetLogType")]
        public IActionResult GetLogType()
        {
            string qry = @"SELECT DISTINCT VTYPE AS TYPE FROM TBLLOG WHERE CMP_ID = " + auth.CmpId + " AND LOCID = '" + auth.LocId + "' AND FINID = " + auth.FinId + "";

            string result = JsonConvert.SerializeObject(_dataLogic.LoadData(qry));
            return Ok(result);
        }

        [HttpDelete("DeleteUserLog")]
        public IActionResult DeleteUserLog(int id, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.TblLogs.Where(x => x.Id == id && x.Uid == userId && x.CmpId == auth.CmpId && x.Locid == auth.LocId && x.Finid == auth.FinId).ExecuteDelete();
                _context.SaveChanges();
                transaction.Commit();
                return Ok(true);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return Ok(false);
                throw;
            }
        }


        #endregion

        #region APP SLIDER

        [HttpGet("GetSlider")]
        public IActionResult GetSlider()
        {
            var slider = _context.TblAppSliders.Where(x => x.CompId == auth.CmpId).Select(y => new { y.Id, y.Sort, path = "Companies/" + auth.CmpName + "/Slider", fileName = y.SliderPath }).OrderBy(z => z.Sort).ToList();
            return Ok(slider);
        }

        [HttpPost("UpdateSorting")]
        public IActionResult UpdateSorting([FromBody] List<TblAppSlider> slider)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                foreach (var item in slider)
                {
                    TblAppSlider slid = _context.TblAppSliders.Where(x => x.CompId == auth.CmpId && x.Id == item.Id).FirstOrDefault();
                    slid.Sort = item.Sort;
                    _context.TblAppSliders.Update(slid);
                }

                _context.SaveChanges();
                transaction.Commit();
                return Ok(true);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return Ok(false);
                throw;
            }
        }

        [HttpDelete("DeleteSlider")]
        public IActionResult DeleteSlider(int id, string name, string path)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                string webRootPath = _hostingEnvironment.WebRootPath;

                var upload = Path.Combine(webRootPath, path);

                if (Directory.Exists(upload))
                {
                    string[] filesLoc = Directory.GetFiles(upload);

                    foreach (string file in filesLoc)
                    {
                        if (Path.GetFileName(file) == name)
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }

                _context.TblAppSliders.Where(x => x.CompId == auth.CmpId && x.Id == id).ExecuteDelete();
                _context.SaveChanges();
                transaction.Commit();
                return Ok(true);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return Ok(false);
                throw;
            }
        }

        [HttpPost("FileUpload")]
        public IActionResult FileUpload([FromForm] IFormFile image)
        {
            if (image != null)
            {
                if (image.Length > 0)
                {
                    using var transaction = _context.Database.BeginTransaction();

                    try
                    {
                        var max = (_context.TblAppSliders.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.Id) ?? 0) + 1;
                        string webRootPath = _hostingEnvironment.WebRootPath;

                        var upload = Path.Combine(webRootPath, "Companies/" + auth.CmpName + "/Slider");

                        var extension = Path.GetExtension(image.FileName);
                        var fileName = max + Path.GetFileName(image.FileName);

                        if (!Directory.Exists(upload))
                        {
                            Directory.CreateDirectory(upload);
                        }

                        using (var filesStream = new FileStream(Path.Combine(upload, fileName), FileMode.Create))
                        {
                            image.CopyTo(filesStream);
                        }

                        _context.TblAppSliders.Add(new TblAppSlider
                        {
                            CompId = auth.CmpId,
                            Sort = (_context.TblAppSliders.Where(x => x.CompId == auth.CmpId).Max(x => (int?)x.Sort) ?? 0) + 1,
                            SliderPath = fileName,
                        });

                        _context.SaveChanges();
                        transaction.Commit();
                        return Ok(true);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return Ok(false);
                        throw;
                    }
                }
            }
            return Ok(false);
        }

        #endregion

        #region SECURITY SYSTEM

        #region  PERMISSION

        [HttpPost("AllowDashboard")]
        public IActionResult AllowDashboard(bool status, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                User user = _context.Users.Where(x => x.Id == userId && x.CmpId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();
                if (user != null)
                {
                    user.Dashboard = status;
                    _context.Update(user);
                    _context.SaveChanges();
                }

                transaction.Commit();
                return Ok(true);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return Ok(false);
            }
        }

        [HttpPost("SaveUpdateAllowForm")]
        public IActionResult SecurityUpdate(List<Tblallowfrm> vM, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Tblallowfrms.Where(x => x.Userid == userId && x.CompId == auth.CmpId).ExecuteDelete();

                if (vM.Count > 0)
                {
                    foreach (var item in vM)
                    {
                        _context.Tblallowfrms.Add(new Tblallowfrm
                        {
                            Menuid = item.Menuid,
                            Userid = item.Userid,
                            CompId = auth.CmpId
                        });
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                return Ok(true);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return Ok(false);
            }
        }

        [HttpGet("GetAllowForm")]
        public IActionResult GetAllowForms(int userId)
        {
            var allowForms = _context.Tblallowfrms.Where(x => x.Userid == userId && x.CompId == auth.CmpId).ToList();
            var dashboard = _context.Users.Where(x => x.Id == userId && x.CmpId == auth.CmpId && x.LocId == auth.LocId).Select(x => x.Dashboard).FirstOrDefault();

            dynamic result = new System.Dynamic.ExpandoObject();
            result.allowForms = allowForms;
            result.dashboard = dashboard;

            return Ok(result);
        }

        [HttpGet("GetAllowMenu")]
        public IActionResult GetAllowMenu()
        {
            return Ok(_context.Tblallowfrms.Where(x => x.Userid == auth.UserId && x.CompId == auth.CmpId).ToList());
        }


        #endregion

        #region VOUCHER TYPE AUTH

        [HttpGet("GetVchData")]
        public IActionResult GetVchData(int userId)
        {
            string qry = $@"SELECT VCHTYPE, ''  CANFEED, '' CANVERIFY, '' CANUNVERIFY, '' CANAPPROVE, '' CANUNAPPROVE, '' CANAUDIT,'' CANUNAUDIT 
            FROM TBLVCHTYPES
            WHERE CMPID = {auth.CmpId} and VCHTYPE NOT IN((SELECT VCHTYPE FROM TBLUSERVCHTYPES WHERE UID = {userId}))
            UNION ALL
            SELECT VCHTYPE, CANFEED, CANVERIFY, CANUNVERIFY, CANAPPROVE, CANUNAPPROVE, CANAUDIT, CANUNAUDIT 
            FROM TBLUSERVCHTYPES 
            WHERE UID = {userId} ORDER BY VCHTYPE";

            string result = JsonConvert.SerializeObject(_dataLogic.LoadData(qry));
            return Ok(result);
        }

        [HttpPost("SaveAllowVchType")]
        public IActionResult SaveAllowVchType([FromBody] List<VoucherTypeVM> vM)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                VoucherTypeVM fr = vM.First();
                _context.TblUserVchTypes.Where(x => x.Uid == fr.UId).ExecuteDelete();

                foreach (var item in vM)
                {
                    _context.TblUserVchTypes.Add(new TblUserVchType
                    {
                        Uid = item.UId,
                        VchType = item.VchType,
                        CanFeed = item.StopEntry,
                        CanVerify = item.CanVerify,
                        CanUnVerify = item.CanUnVerify,
                        CanApprove = item.CanApprove,
                        CanUnApprove = item.CanUnApprove,
                        CanAudit = item.CanAudit,
                        CanUnAudit = item.CanUnAudit
                    });
                }

                transaction.Commit();
                _context.SaveChanges();
                return Ok(true);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return Ok(true);
                throw;
            }
        }

        #endregion

        #region MONTH CLOSE

        [HttpGet("LastCloseMonth")]
        public IActionResult LastCloseMonth()
        {
            string qry = $@"SELECT * FROM TBLMONTHCLOSE WHERE COMP_ID = {auth.CmpId} AND FINID = {auth.FinId}";
            string result = JsonConvert.SerializeObject(_dataLogic.LoadData(qry));
            return Ok(result);
        }

        [HttpPost("SaveUpdateMonthClose")]
        public IActionResult SaveUpdateMonthClose(int monthClose, int monthOpen, DateTime autoClose)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DateTime monthOp = new DateTime(2050, 01, 01);
                bool closing = true;

                if (monthOpen != 0)
                {
                    monthOp = new DateTime(DateTime.Now.Year, monthOpen, 01);
                    closing = false;
                }

                TblMonthClose close = _context.TblMonthCloses.Where(x => x.CompId == auth.CmpId && x.FinId == auth.FinId).FirstOrDefault();

                if (close == null)
                {
                    _context.TblMonthCloses.Add(new TblMonthClose
                    {
                        CompId = auth.CmpId,
                        FinId = auth.FinId,
                        MonthClosingDate = monthClose,
                        AutoClosingDate = autoClose,
                        MonthOpening = monthOp,
                        Closing = closing
                    });
                }
                else
                {
                    close.MonthClosingDate = monthClose;
                    close.AutoClosingDate = autoClose;
                    close.MonthOpening = monthOp;
                    close.Closing = closing;
                    _context.TblMonthCloses.Update(close);
                }

                _context.SaveChanges();
                transaction.Commit();
                return Ok(true);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return Ok(false);
                throw;
            }
        }

        #endregion

        #region DAY CLOSE

        [HttpGet("LastCloseDate")]
        public IActionResult LastCloseDate(string locId)
        {
            string qry = $@"SELECT ISNULL(CONVERT(VARCHAR(11),DAYCLOSE, 103), CONVERT(VARCHAR(11),GETDATE(), 103)) AS DATE FROM TBLDAYCLOSE WHERE COMP_ID = {auth.CmpId} AND FINID = {auth.FinId} AND LOCID = '{locId}'";
            string result = JsonConvert.SerializeObject(_dataLogic.LoadData(qry));
            return Ok(result);
        }

        [HttpPost("SaveUpdateDayClose")]
        public IActionResult SaveUpdateDayClose(DateTime dayClose, DateTime dtNow, string status, string locId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                TblDayClose close = _context.TblDayCloses.Where(x => x.CompId == auth.CmpId && x.FinId == auth.FinId && x.LocId == locId).FirstOrDefault();

                if (close == null)
                {
                    _context.TblDayCloses.Add(new TblDayClose
                    {
                        CompId = auth.CmpId,
                        FinId = auth.FinId,
                        LocId = locId,
                        DayClose = dayClose,
                        Closing = true
                    });
                }
                else
                {
                    close.DayClose = Convert.ToDateTime(dayClose);
                    _context.TblDayCloses.Update(close);
                }
                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Day Close", $"Day {status} Date({dayClose.ToString("dd/MM/yyyy")})", 0, dtNow, 0, 0, 0, dtNow);
                return Ok(true);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return Ok(false);
                throw;
            }
        }

        #endregion

        #endregion

        #region LOGICS

        [HttpPost("InsertTransmain")]
        public IActionResult InsertTransmain1()
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var main = _context.TblTransVches.Where(x => x.CmpId == auth.CmpId && new[] { "SP-RAW" }.Contains(x.VchType)).Select(y => new { y.LocIdN, y.VchType, y.VchNo, y.VchDate }).Distinct().OrderBy(z => z.LocIdN).ThenBy(z => z.VchType).ThenBy(z => z.VchNo).ToList();

                foreach (var item in main)
                {
                    _context.TransMains.Where(x => x.CmpId == auth.CmpId && x.VchType == item.VchType && x.VchNo == item.VchNo && x.LocId == item.LocIdN).ExecuteDelete();
                    _context.TransMains.Add(new TransMain
                    {
                        VchNo = Convert.ToInt32(item.VchNo),
                        VchType = item.VchType,
                        VchDateM = item.VchDate,
                        AppBy = 6,
                        Aprove = true,
                        Verify = 1,
                        VerifyBy = 6,
                        AuditBy = 1,
                        AuditByN = 6,
                        //Apploc = fr.Tag,
                        CmpId = auth.CmpId,
                        LocId = item.LocIdN,
                        FinId = auth.FinId,
                    });
                }
                _context.SaveChanges();
                transaction.Commit();
                return Ok(true);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return Ok(false);
                throw;
            }
        }

        #endregion
    }
}
