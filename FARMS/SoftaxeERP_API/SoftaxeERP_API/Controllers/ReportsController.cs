using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoftaxeERP_API.Services;

namespace SoftaxeERP_API.Controllers
{


    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ReportsController : ControllerBase
    {

        private readonly IReports _rpt;

        public ReportsController(IReports test)
        {
            _rpt = test;
        }


        #region Cheque Availability Report

        //Get Expense Budget Report
        [HttpGet("GetChequeAvailability")]
        public IActionResult GetChequeAvailability(string LocId, string filterBy)
        {
            var data = _rpt.GetChequeAvailability(LocId, filterBy);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Cheque Books Report

        //Get Expense Budget Report
        [HttpGet("GetChqBooksReport")]
        public IActionResult GetChqBooksReport(int FinId, string LocId, string BFrom, string BTo, DateTime FromDate, DateTime ToDate, int CompId)
        {
            var data = _rpt.GetChqBooksReport(FinId, LocId, BFrom, BTo, FromDate, ToDate, CompId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Purchase Contract Detail

        //Get Purchase Contract Detail
        [HttpGet("GetPurchaseContractDetail")]
        public IActionResult GetPurchaseContractDetail(DateTime FromDate, DateTime ToDate)
        {
            var data = _rpt.GetPurchaseContractDetail(FromDate, ToDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Purchase Contract Report

        //Get Purchase Contract Detail
        [HttpGet("GetPurchaseContractReport")]
        public IActionResult GetPurchaseContractReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category, string cancel)
        {
            var data = _rpt.GetPurchaseContractReport(VchType, PFrom, PTo, IFrom, ITo, FinId, LocId, FromDate, ToDate, Category, cancel);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Purchase Detail Report

        //Get Purchase Detail Report
        [HttpGet("GetPurchaseDetailReport")]
        public IActionResult GetPurchaseDetailReport(string VchType, int VchnoFrom, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId)
        {
            var data = _rpt.GetPurchaseDetailReport(VchType, VchnoFrom, PFrom, PTo, IFrom, ITo, FinId, LocId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Purchase Contract Canceled Report

        //Get Purchase Contract Canceled Report
        [HttpGet("GetPurContractCanceledReport")]
        public IActionResult GetPurContractCanceledReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category)
        {
            var data = _rpt.GetPurContractCanceledReport(VchType, PFrom, PTo, IFrom, ITo, FinId, LocId, FromDate, ToDate, Category);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Purchase Contract Status Report

        //Get Purchase Contract Status Report
        [HttpGet("GetPurContractStatusReport")]
        public IActionResult GetPurContractStatusReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category)
        {
            var data = _rpt.GetPurContractStatusReport(VchType, PFrom, PTo, IFrom, ITo, FinId, LocId, FromDate, ToDate, Category);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Purchase Contract Pending Report

        //Get Purchase Contract Pending Report
        [HttpGet("GetPurContractPendingReport")]
        public IActionResult GetPurContractPendingReport(string VchType, string PFrom, string PTo, string IFrom, string ITo, int FinId, string LocId, DateTime FromDate, DateTime ToDate, string Category, string cancel)
        {
            var data = _rpt.GetPurContractPendingReport(VchType, PFrom, PTo, IFrom, ITo, FinId, LocId, FromDate, ToDate, Category, cancel);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Product Wise Sale Report

        //Get Purchase Contract Detail
        [HttpGet("GetProductWiseSaleReport")]
        public IActionResult GetProductWiseSaleReport(string VchType, string InwardType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, string PFrom, string PTo, string IFrom, string ITo, DateTime FromDate, DateTime ToDate, int FinId, string LocId)
        {
            var data = _rpt.GetProductWiseSaleReport(VchType, InwardType, VchNoFrom, VchNoTo, CatFrom, CatTo, PFrom, PTo, IFrom, ITo, FromDate, ToDate, FinId, LocId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Product Party Wise Sale Report

        //Get Product Party Wise Sale Report
        [HttpGet("GetProdPartyWiseSaleReport")]
        public IActionResult GetProdPartyWiseSaleReport(string VchType, string InwardType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, string PFrom, string PTo, string IFrom, string ITo, DateTime FromDate, DateTime ToDate, int FinId, string LocId)
        {
            var data = _rpt.GetProdPartyWiseSaleReport(VchType, InwardType, VchNoFrom, VchNoTo, CatFrom, CatTo, PFrom, PTo, IFrom, ITo, FromDate, ToDate, FinId, LocId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Daily Product Wise Sale Report

        //Get Purchase Contract Detail
        [HttpGet("GetDailyProductWiseSaleReport")]
        public IActionResult GetDailyProductWiseSaleReport(string VchType, string InwardType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, string PFrom, string PTo, string IFrom, string ITo, DateTime FromDate, DateTime ToDate, int FinId, string LocId)
        {
            var data = _rpt.GetDailyProductWiseSaleReport(VchType, InwardType, VchNoFrom, VchNoTo, CatFrom, CatTo, PFrom, PTo, IFrom, ITo, FromDate, ToDate, FinId, LocId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Daily Weight Bridge Inward Report

        //Get Weight Bridge Inward Report
        [HttpGet("GetWeightBridgeInwardReport")]
        public IActionResult GetWeightBridgeInwardReport(string VchType, string VchNoFrom, string VchNoTo, string CatFrom, string CatTo, DateTime FromDate, DateTime ToDate, int FinId, string LocId, string PFrom, string PTo, string IFrom, string ITo)
        {
            var data = _rpt.GetWeightBridgeInwardReport(VchType, VchNoFrom, VchNoTo, CatFrom, CatTo, FromDate, ToDate, FinId, LocId, PFrom, PTo, IFrom, ITo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Daily Purchase Report

        //Get Daily Purchase Report
        [HttpGet("GetDailyPurchaseReport")]
        public IActionResult GetDailyPurchaseReport(DateTime FromDate, DateTime ToDate, string PartyCode, string ItemCode, string Approvel)
        {
            var data = _rpt.GetDailyPurchaseReport(FromDate, ToDate, PartyCode, ItemCode, Approvel);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Daily Purchase Activity Report

        //Get Daily Purchase Activity Report
        [HttpGet("GetDailyPurchaseActivityReport")]
        public IActionResult GetDailyPurchaseActivityReport(DateTime FromDate, DateTime ToDate, int finid, string locid, int cmpId)
        {
            var data = _rpt.GetDailyPurchaseActivityReport(FromDate, ToDate, finid, locid, cmpId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Daily Sale Activity Report

        //Get Daily Sale Activity Report
        [HttpGet("GetDailySaleActivityReport")]
        public IActionResult GetDailySaleActivityReport(DateTime FromDate, DateTime ToDate, int finid, string locid, int cmpId)
        {
            var data = _rpt.GetDailySaleActivityReport(FromDate, ToDate, finid, locid, cmpId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion


        #region Inward Purchase Status

        [HttpGet("GetInwardPurchaseStatusReport")]
        public IActionResult GetInwardPurchaseStatusReport(DateTime fromDate, DateTime toDate)
        {
            var data = _rpt.GetInwardPurchaseStatusReport(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion


        [HttpGet("GetItemList")]
        public IActionResult GetItemList()
        {
            var data = _rpt.GetItemList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetCostCenter")]
        public IActionResult GetCostCenter()
        {
            var data = _rpt.GetCostCenter();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetLevel5List")]
        public IActionResult GetLevel5List(string tag)
        {
            var data = _rpt.GetLevel5List(tag);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpGet("GetMonthWiseActivity")]
        public IActionResult GetMonthWiseActivity(DateTime fromYear, DateTime toYear)
        {
            var data = _rpt.GetMonthWiseActivity(fromYear, toYear);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

    }
}
