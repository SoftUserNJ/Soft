using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoftaxeERP_API.Services;
using SoftaxeERP_API.VM;

namespace SoftaxeERP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IApproval _approval;
        private readonly ISaleBooking _saleBooking;

        public ApprovalController(IApproval approval, ISaleBooking saleBooking)
        {
            _approval = approval;
            _saleBooking = saleBooking;
        }

        [HttpGet("GetPoVch")]
        public IActionResult GetPoVch(DateTime fromDate, DateTime toDate)
        {
            var data = _approval.GetPoVch(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("UpdatePOVoucherStatus")]
        public IActionResult UpdatePOVoucherStatus(List<POApprovalVM> status)
        {
            var data = _approval.UpdatePOVoucherStatus(status);
            return Ok(data);
        }

        #region Gate Pass Inward Approval

        [HttpGet("GetGPIVch")]
        public IActionResult GetGPIVch(DateTime fromDate, DateTime toDate, bool unapproved, bool approved, bool rejected, bool all)
        {
            var data = _approval.GetGPIVch(fromDate, toDate,  unapproved,  approved,  rejected,  all);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetGPIVchDetail")]
        public IActionResult GetGPIVchDetail(int vchNo, string vchType)
        {
            var data = _approval.GetGPIVchDetail(vchNo, vchType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }


        [HttpPost("UpdateGPIVoucherStatus")]
        public IActionResult UpdateGPIVoucherStatus(List<GPIApprovalVM> status)
        {
            var data = _approval.UpdateGPIVoucherStatus(status);
            return Ok(data);
        }

        #endregion

        #region Maize-Rate-Approval
        [HttpPost("UpdateMaizeRateStatus")]
        public IActionResult UpdateMaizeRateStatus(List<MaizeRateApprovalVM> status)
        {
            var data = _approval.UpdateMaizeRateStatus(status);
            return Ok(data);
        }

        #endregion


        #region Party Terms Approval

        [HttpGet("GetPartyTermsList")]
        public IActionResult GetPartyTermsList()
        {
            var data = _saleBooking.GetPartyTermsList();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("UpdatePartyTermCondition")]
        public IActionResult UpdatePartyTermCondition(List<PartyTermConditionsApprovalVM> status)
        {
            var data = _approval.UpdatePartyTermCondition(status);
            return Ok(data);
        }

        #endregion

        #region DO-Approval

        [HttpGet("GetDOApprovalList")]
        public IActionResult GetDOApprovalList(DateTime fromDate, DateTime toDate, string vchType)
        {
            var data = _approval.GetDOApprovalList(fromDate, toDate, vchType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("UpdateDoStatus")]
        public IActionResult UpdateDoStatus(List<DOApprovalVM> status)
        {
            var data = _approval.UpdateDoStatus(status);
            return Ok(data);
        }


        #endregion
    }
}