using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoftaxeERP_API.Services;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IChartOfAccount _coa;
        private readonly IAccountsOpening _acc;
        private readonly IJournalVoucher _jv;
        private readonly ITrialBalance _tb;
        private readonly IPaymentReceipts _pr;
        private readonly IPaymentReceiptsStatus _prs;
        private readonly IApproval _app;
        private readonly IAccounts _accounts;

        public AccountsController(IChartOfAccount coa, IAccountsOpening acc, IJournalVoucher jv, ITrialBalance tb, IPaymentReceipts apr, IPaymentReceiptsStatus prs, IApproval app, IAccounts accounts)
        {
            _coa = coa;
            _acc = acc;
            _jv = jv;
            _tb = tb;
            _pr = apr;
            _prs = prs;
            _app = app;
            _accounts = accounts;
        }

        [HttpGet("GetAccountsList")]
        public IActionResult GetAccountsList(string module)
        {
            var data = _pr.GetAccountList(module);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        [HttpGet("GetBankCash")]
        public IActionResult GetBankCash(string vchType)
        {
            var data = _pr.GetBankCash(vchType);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        [HttpGet("GetGroup")]
        public IActionResult GetGroup()
        {
            var data = _prs.GetGroup();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        [HttpGet("GetCompany")]
        public IActionResult GetCompany(int groupId)
        {
            var data = _prs.GetCompany(groupId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetMaxNumber")]
        public IActionResult GetARMaxNumber(string vchType)
        {
            var data = _pr.GetMaxNumber(vchType);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        [HttpGet("GetFinDate")]
        public IActionResult GetFinDate()
        {
            var data = _acc.GetFinDate();
            return Ok(data);
        }


        #region Payment Freight

        [HttpGet("GetVchFec")]
        public IActionResult GetVchFec(DateTime vchDate, DateTime fromDate, DateTime toDate, string tag)
        {
            var data = _accounts.GetVchFec(vchDate, fromDate, toDate, tag);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion


        #region CHART OF ACCOUNT

        [HttpGet("GetCOA")]
        public IActionResult GetCOA()
        {
            var data = _coa.GetCOA();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetLevels")]
        public IActionResult GetLevels(string code, string status)
        {
            DataTable data = new();

            if (status == "L1")
            {
                data = _coa.GetLevel1();
            }
            else if(status == "L2")
            {
                data = _coa.GetLevel2(code);
            }
            else if(status == "L3")
            {
                data = _coa.GetLevel3(code);
            }
            else if(status == "L4")
            {
                data = _coa.GetLevel4(code);
            }
            else if(status == "L5")
            {
                data = _coa.GetLevel5(code);
            }

            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdateLevels")]
        public IActionResult SaveUpdateLevels([FromBody] CoaVM coa)
        {
            string result = "";
            if (coa.Status == "L1")
            {
                result = _coa.SaveUpdateLevel1(coa.Code, coa.Name, coa.DtNow);
            }
            else if (coa.Status == "L2")
            {
                result = _coa.SaveUpdateLevel2(coa.PreLvl, coa.Code, coa.Name, coa.DtNow);
            }
            else if (coa.Status == "L3")
            {
                result = _coa.SaveUpdateLevel3(coa.PreLvl, coa.Code, coa.Name, coa.DtNow);
            }
            else if (coa.Status == "L4")
            {
                result = _coa.SaveUpdateLevel4(coa.PreLvl, coa.Code, coa.Name, coa.Tag, coa.Tag1, coa.SaleCode, coa.MapCode, coa.DtNow);
            }
            else if (coa.Status == "L5")
            {
                result = _coa.SaveUpdateLevel5(coa.PreLvl, coa.Code, coa.Name, coa.MapCode, coa.DtNow);
            }

            return Ok(result);
        }

        [HttpDelete("DeleteLevels")]
        public IActionResult DeleteLevels(string code, string status, DateTime dtNow)
        {
            string result = "false";
            if (status == "L1")
            {
                result = _coa.DeleteLevel1(code, dtNow);
            }
            else if (status == "L2")
            {
                result = _coa.DeleteLevel2(code, dtNow);
            }
            else if (status == "L3")
            {
                result = _coa.DeleteLevel3(code, dtNow);
            }
            else if (status == "L4")
            {
                result = _coa.DeleteLevel4(code, dtNow);
            }
            else if (status == "L5")
            {
                result = _coa.DeleteLevel5(code, dtNow);
            }

            return Ok(result);
        }

        #endregion

        #region ACCOUNT OPENING

        [HttpGet("GetAccountOP")]
        public IActionResult GetAccountOP()
        {
            var data = _acc.GetAccountOP();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveAccountOP")]
        public IActionResult SaveAccountOP(List<AccountOPVM> accounts)
        {
            bool result = _acc.SaveAccountOP(accounts);
            return Ok(result);
        }

        [HttpPost("UpdateBalanceSheet")]
        public IActionResult UpdateBalanceSheet()
        {
            bool result = _acc.UpdateBalanceSheet();
            return Ok(result);
        }

        #endregion

        #region ACCOUNT PAYMENT / RECEIPTS

        [HttpGet("GetPRVoucher")]
        public IActionResult GetPRVoucher(DateTime fromDate, DateTime toDate, string tag, string module)
        {
            var data = _pr.GetVoucher(fromDate, toDate, tag, module);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
             
        [HttpPost("SaveUpdatePR")]
        public IActionResult SaveUpdatePR([FromBody] PRListVM payment)
        {
            var result = _pr.SaveUpdate(payment);
            return Ok(result);
        }

        [HttpGet("EditPR")]
        public IActionResult EditPR(int vchNo, string vchType, string tag)
        {
            var data = _pr.EditVoucher(vchNo, vchType, tag);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpDelete("DeletePR")]
        public IActionResult DeletePR(int vchNo, string vchType, DateTime dtNow)
        {
            bool result = _pr.Delete(vchNo, vchType, dtNow);
            return Ok(result);
        }

        #endregion

        #region JOURNAL VOUCHER

        [HttpGet("GetAccountHead")]
        public IActionResult GetAccountHead()
        {
            var data = _jv.GetAccountHeads();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveUpdateJV")]
        public async Task<IActionResult> SaveUpdateJV(List<JournalVoucherVM> journalVouchers)
        {
            var result = await _jv.SaveUpdateJV(journalVouchers);
            return Ok(result);
        }

        [HttpGet("GetJV")]
        public IActionResult GetJV(DateTime fromDate, DateTime toDate, string vchType)
        {
            var data = _jv.GetSavedData(fromDate, toDate, vchType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpDelete("DeleteJV")]
        public IActionResult DeleteJV(int vchNo, string vchType, DateTime dtNow)
        {
            string result = _jv.DeleteVoucher(vchNo, vchType, dtNow);
            return Ok(result);
        }

        [HttpGet("EditJV")]
        public IActionResult EditJV(int vchNo, string vchType)
        {
            var data = _jv.EditVchData(vchNo, vchType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        #endregion

        #region TRIAL BALANCE

        [HttpGet("GetTrialBalance")]
        public IActionResult GetTrialBalance(DateTime fromDate, DateTime toDate, string filterBy, string locId)
        {
            DataTable data = _tb.GetTrialBalance(fromDate, toDate, filterBy, locId);
            
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region PAYMENT RECEIPTS STATUS

        [HttpGet("GetPRParty")]
        public IActionResult GetPRParty()
        {
            DataTable data = _prs.GetPRParty();
            
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetPRPBankCash")]
        public IActionResult GetPRPBankCash()
        {
            DataTable data = _prs.GetPRPBankCash();
            
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetPaymentReceipts")]
        public IActionResult GetPaymentReceipts(DateTime fromDate, DateTime toDate, int companyId)
        {
            DataTable data = _prs.GetPaymentReceipts(fromDate, toDate, companyId);
            
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region ACCOUNT LEDGER

       [HttpGet("GetAccountLedgerList")]
        public IActionResult GetAccountLedgerList()
        {
            var data = _acc.GetAccountLedgerList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region APPROVAL SYSTEM

        [HttpGet("GetAllowVchType")]
        public IActionResult GetAllowVchType(string tag)
        {
            var data = _app.GetAllowVchType(tag);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("TopMultiData")]
        public IActionResult TopMultiData()
        {
            var result = _app.TopMultiData();
            return Ok(result);
        }

        [HttpGet("GetVchApproval")]
        public IActionResult GetVchApproval(DateTime fromDate, DateTime toDate, string status, string tag, string locId, string userId)
        {
            var data = _app.GetVchApproval(fromDate, toDate, status, tag, locId, userId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetVchDetail")]
        public IActionResult GetVchDetail(string vchType, string vchNo, string locId, DateTime vchDate)
        {
            var data = _app.GetVchDetail(vchType, vchNo, locId, vchDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveVouchersApproval")]
        public IActionResult SaveVouchersApproval(List<ApprovalVM> vM)
        {
            var result = _app.SaveVouchersApproval(vM);
            return Ok(result);
        }

        [HttpGet("GetVchData")]
        public IActionResult GetVchData(string locId, string userId)
        {
            var data = _app.GetVchData(locId, userId);
            return Ok(data);
        }

        [HttpGet("GetVchTypes")]
        public IActionResult GetVchTypes()
        {
            var data = _app.GetVchTypes();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("GetVchTypesDataUpdate")]
        public IActionResult GetVchTypesDataUpdate(int id, string name, string description, string verifyName, bool verifyRequired, string approvalName, bool approvalRequired, string auditName, string lastText)
        {
            var result = _app.GetVchTypesDataUpdate(id, name, description, verifyName, verifyRequired, approvalName, approvalRequired, auditName, lastText);
            return Ok(result);
        }

        #endregion

        #region TAX

        [HttpGet("GetTax")]
        public IActionResult GetTax(string tag)
        {
            var data = _pr.GetTax(tag);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
             
        [HttpPost("AddUpdateTax")]
        public IActionResult AddUpdateTax(double tax, string tag)
        {
            bool result = _pr.AddUpdateTax(tax, tag);
            return Ok(result);
        }

        [HttpDelete("DeleteTax")]
        public IActionResult DeleteTax(double tax, string tag)
        {
            string result = _pr.DeleteTax(tax, tag);
            return Ok(result);
        }

        #endregion

        #region FILE UPLOAD

        [HttpPost("FileUpload")]
        public IActionResult FileUpload([FromForm]int vchNo, [FromForm]string vchType, [FromForm]IFormFile file)
        {
            bool result = _pr.FileUploads(vchNo, vchType, file);
            return Ok(result);
        }

        [HttpGet("GetFiles")]
        public IActionResult GetFiles(string vchType, int vchNo)
        {
            var result = _pr.GetFiles(vchType, vchNo);
            return Ok(result);
        }

        [HttpDelete("DeleteFile")]
        public IActionResult DeleteFile(string name, string path)
        {
            bool result = _pr.DeleteFile(name, path);
            return Ok(result);
        }

        #endregion
    }
}
