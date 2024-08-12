using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.Services;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly ICustomerSupplier _supplier;
        private readonly IBillDueStatus _bill;
        private readonly IPostDateCheque _pd;
        private readonly IPurchaseInvoice _pur;
        private readonly IPaymentReceipts _pr;
        private readonly IGatePassInward _gp;
        private readonly ILab _lab;

        public PurchaseController(ICustomerSupplier supplier, IBillDueStatus bill, IPostDateCheque pd, IPurchaseInvoice pur, IPaymentReceipts pr, IGatePassInward gp, ILab lab)
        {
            _supplier = supplier;
            _bill = bill;
            _pd = pd;
            _pur = pur;
            _pr = pr;
            _gp = gp;
            _lab = lab;
        }

        #region MAIN AREA

        [HttpGet("GetMainArea")]
        public IActionResult GetMainArea()
        {
            var data = _supplier.GetMainArea("Supplier");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateMainArea")]
        public IActionResult AddUpdateMainArea(int id, string name, DateTime dtNow)
        {
            bool data = _supplier.AddUpdateMainArea(id, name, "Supplier", dtNow);

            return Ok(data);
        }

        [HttpDelete("DeleteMainArea")]
        public IActionResult DeleteMainArea(int id, DateTime dtNow)
        {
            var data = _supplier.DeleteMainArea(id, "Supplier", dtNow);

            return Ok(data);
        }

        #endregion

        #region SUB AREA

        [HttpGet("GetSubArea")]
        public IActionResult GetSubArea(int mainAreaId)
        {
            var data = _supplier.GetSubArea(mainAreaId, "Supplier");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateSubArea")]
        public IActionResult AddUpdateSubArea(int mainAreaId, int id, string name, DateTime dtNow)
        {
            bool data = _supplier.AddUpdateSubArea(mainAreaId, id, name, "Supplier", dtNow);

            return Ok(data);
        }

        [HttpDelete("DeleteSubArea")]
        public IActionResult DeleteSubArea(int mainAreaId, int id, DateTime dtNow)
        {
            var data = _supplier.DeleteSubArea(mainAreaId, id, "Supplier", dtNow);

            return Ok(data);
        }

        #endregion

        #region SUPPLIER

        [HttpGet("GetSupplier")]
        public IActionResult GetSupplier(bool status)
        {
            var data = _supplier.GetCustomerSupplier("Supplier", status);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateSupplier")]
        public IActionResult AddUpdateSupplier(CustomerSupplierVM customerSupplierVM)
        {
            string data = _supplier.AddUpdateCustomerSupplier(customerSupplierVM);
            return Ok(data);
        }

        [HttpDelete("DeleteSupplier")]
        public IActionResult DeleteSupplier(string code, DateTime dtNow)
        {
            var data = _supplier.DeleteCustomerSupplier(code, "Supplier", dtNow);

            return Ok(data);
        }

        #endregion

        #region SUPPLIER LEDGER

        [HttpGet("GetSupplierLedger")]
        public IActionResult GetSupplierLedger()
        {
            var data = _supplier.GetLedgerList("C");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        #endregion

        #region SUPPLIER AGING

        [HttpGet("GetAging")]
        public IActionResult GetAging()
        {
            var data = _supplier.GetAging("Supplier");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdateAging")]
        public IActionResult SaveUpdateAging(int d1, int d2, int d3, int d4, int d5, int d6, int d7, DateTime dtNow)
        {
            var result = _supplier.AddUpdateAging(d1, d2, d3, d4, d5, d6, d7, "Supplier", dtNow);
            return Ok(result);
        }

        #endregion

        #region BILL DUE STATUS

        [HttpGet("GetBillDueStatus")]
        public IActionResult GetBillDueStatus(DateTime fromDate, DateTime toDate, string status, int cmpId, string locId)
        {
            var data = _bill.PurchaseBillDueStatus(fromDate, toDate, status, cmpId, locId);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetRecAmount")]
        public IActionResult GetRecAmount(int invoiceNo, string vchType)
        {
            var data = _bill.GetRecAmount(invoiceNo, vchType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        #endregion

        #region POST DATED CHEQUE

        [HttpGet("GetPDParty")]
        public IActionResult GetPDParty()
        {
            var data = _pd.GetPDParty("BP");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetPDBank")]
        public IActionResult GetPDBank()
        {
            var data = _pd.GetPDBank();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetPDChequeList")]
        public IActionResult GetPDChequeList(DateTime fromDate, DateTime toDate, string status)
        {
            var data = _pd.GetPDChequeList(fromDate, toDate, status, "BP");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SavePDCheque")]
        public IActionResult SavePDCheque(List<PDChequeVM> vM)
        {
            var data = _pd.SavePDCheque(vM);
            return Ok(data);
        }

        #endregion

        #region PURCHASE PAYMENT / RECEIPTS

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

        [HttpGet("InvoiceList")]
        public IActionResult InvoiceList(string code)
        {
            var data = _pr.InvoiceList("purchase", code);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("EditPR")]
        public IActionResult EditPR(int vchNo, string vchType, string tag)
        {
            var data = _pr.EditVoucher(vchNo, vchType, tag);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("EditPartyPR")]
        public IActionResult EditPartyPR(int vchNo, string vchType, string partyCode, string status)
        {
            var data = _pr.EditPartyData(vchNo, vchType, partyCode, status);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpDelete("DeletePR")]
        public IActionResult DeletePR(int vchNo, string vchType, DateTime dtNow)
        {
            bool result = _pr.Delete(vchNo, vchType, dtNow);
            return Ok(result);
        }


        [HttpGet("CallOldBalance")]
        public IActionResult CallOldBalance(string code)
        {
            double result = _pr.CallOldBalance(code);
            return Ok(result);
        }

        [HttpGet("GetDisData")]
        public IActionResult GetDisData(string vchType, int vchNo)
        {
            var data = _pr.GetDisData(vchType, vchNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveDiscount")]
        public IActionResult SaveDiscount(string vchType, int vchNo, double netDue, decimal discount, decimal disAmount, decimal otherCredit, string remarks, string partyName, DateTime dtNow)
        {
            var data = _pr.SaveDisData(vchType, vchNo, netDue, discount, disAmount, otherCredit, remarks, partyName, dtNow);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region PURCHASE INVOICE

        [HttpGet("PIProductList")]
        public IActionResult PIProductList(int categoryId, string productName, string barCode, DateTime vchDate, string vchType)
        {
            var data = _pur.PIProductList(categoryId, productName, barCode, vchDate, vchType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetPIInvoices")]
        public IActionResult GetPIInvoices(DateTime fromDate, DateTime toDate, string vchType)
        {
            var data = _pur.GetInvoice(fromDate, toDate, vchType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetPIMax")]
        public IActionResult GetPIMax(string vchType)
        {
            var data = _pur.GetMax(vchType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdatePurchaseInvoice")]
        public async Task<IActionResult> SaveUpdatePurchaseInvoice([FromBody] PurchaseVM invoice)
        {
            //List<PurchaseInvoiceVM> deserialObj = JsonConvert.DeserializeObject<List<PurchaseInvoiceVM>>(invoice);
            object result = await _pur.SaveUpdatePIInvoice(invoice);
            return Ok(result);
        }

        [HttpGet("EditPIInvoice")]
        public IActionResult EditPIInvoice(int invNo, string vchType)
        {
            var data = _pur.EditPIInvoice(invNo, vchType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpDelete("DeletePurchaseInvoice")]
        public IActionResult DeletePurchaseInvoice(int invNo, string vchType, DateTime dtNow)
        {
            var result = _pur.DeletePIInvoice(invNo, vchType, dtNow);
            return Ok(result);
        }

        #endregion

        #region TERMS

        [HttpGet("GetTerms")]
        public IActionResult GetTerms()
        {
            var data = _pur.Terms();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateTerms")]
        public IActionResult AddUpdateTerms(int id, string name)
        {
            bool data = _pur.AddUpdateTerms(id, name);

            return Ok(data);
        }

        [HttpDelete("DeleteTerms")]
        public IActionResult DeleteTerms(int id)
        {
            var data = _pur.DeleteTerms(id);

            return Ok(data);
        }

        #endregion

        #region Purchase Order


        [HttpGet("ListVchType")]
        public IActionResult ListVchType()
        {
            var data = _pur.ListVchType();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetCategory")]
        public IActionResult GetCategory()
        {
            var data = _pur.GetCategory();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }


        [HttpGet("GetProductDetails")]
        public IActionResult GetProductDetails(string level4)
        {
            var data = _pur.GetProductDetails(level4);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }


        [HttpGet("GetParty")]
        public IActionResult GetParty()
        {
            var data = _pur.GetParty();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }


        [HttpGet("GetPartySaleDetail")]
        public IActionResult GetPartySaleDetail(string code)
        {
            var data = _pur.GetPartySaleDetail(code);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetBroker")]
        public IActionResult GetBroker()
        {
            var data = _pur.GetBroker();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetUomData")]
        public IActionResult GetUomData()
        {
            var data = _pur.GetUomData();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetLastVchNo")]
        public IActionResult GetLastVchNo()
        {
            var data = _pur.GetLastVchNo();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }


        [HttpGet("GetPoByDate")]
        public IActionResult GetPoByDate(string fromDate, string toDate)
        {
            var data = _pur.GetPoByDate(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }


        [HttpGet("GetPoByPoNo")]
        public IActionResult GetPoByPoNo(int poNo)
        {
            var data = _pur.GetPoByPoNo(poNo);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveBrokerDetails")]
        public IActionResult SaveBrokerDetails(string Names, string Address, string City, string Email, string Phone, string Level4, string Level5)
        {
            var result = _pur.SaveBrokerDetails(Names, Address, City, Email, Phone, Level4, Level5);
            return Ok(result);
        }


        [HttpPost("SaveSupplierDetails")]
        public IActionResult SaveSupplierDetails(string Names, string Address, string City, string Email, string Phone, string Level4, string Level5)
        {
            var result = _pur.SaveSupplierDetails(Names, Address, City, Email, Phone, Level4, Level5);
            return Ok(result);
        }

        [HttpDelete("DeletePurchase")]
        public IActionResult DeletePurchase(int PoNo)
        {
            var result = _pur.DeletePurchase(PoNo);
            return Ok(result);
        }


        [HttpPost("SavePurchaseDetails")]
        public IActionResult SavePurchaseDetails(List<ItemDetail> itemdetails, string VchType, DateTime PoCompDate,
           int BrokerComm, int PoNo, DateTime Podate, string BrokerCommUom, string Remarks, string BagsType,
           string FreightType, DateTime EntryDate, int SuppBrkrComsn, string CrpYear, string pCode, string pSubCode, string bCode, string bSubCode, float incomeTax, int payAfter,
           DateTime DeliveryDate, string InvoiceType, float BagsRate)
        {
            var result = _pur.SavePurchaseDetails(itemdetails, VchType, PoCompDate, BrokerComm, PoNo, Podate, BrokerCommUom, Remarks, BagsType, FreightType, EntryDate, SuppBrkrComsn, CrpYear, pCode, pSubCode, bCode, bSubCode, incomeTax, payAfter, DeliveryDate, InvoiceType, BagsRate);
            return Ok(result);
        }

        [HttpGet("ListUom2")]
        public IActionResult ListUom2()
        {
            var data = _pur.ListUom2();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("EditPurchaseDetails")]
        public IActionResult EditPurchaseDetails(int poNo)
        {
            var data = _pur.EditPurchaseDetails(poNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Daily Consumption


        [HttpGet("GetDailyConsList")]
        public IActionResult GetDailyConsList(DateTime fromDate, DateTime toDate)
        {
            var data = _pur.GetDailyConsList(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpPost("SaveDailyCons")]
        public IActionResult SaveDailyCons([FromBody] List<DailyConsumptionVM> d)
        {
            bool data = _pur.SaveDailyCons(d);

            return Ok(data);
        }

        [HttpGet("GetEditDailyCons")]
        public IActionResult GetEditDailyCons(int jobNo)
        {
            var data = _pur.GetEditDailyCons(jobNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpDelete("DelDailyCons")]
        public IActionResult DelDailyCons(int vchNo)
        {
            var data = _pur.DelDailyCons(vchNo);

            return Ok(data);
        }

        #endregion

        #region Purchase Working


        [HttpGet("GetPendingWorkList")]
        public IActionResult GetPendingWorkList(string locIdUnit)
        {
            DataTable list = _pur.GetPendingWorkList(locIdUnit);
            string result = JsonConvert.SerializeObject(list);
            return Ok(result);
        }

        [HttpGet("GetPendingWorkDetail")]
        public IActionResult GetPendingWorkDetail(int vchNo, bool workDone, string vchType, string locIdUnit)
        {
            DataTable list = _pur.GetPendingWorkDetail(vchNo, vchType, workDone, locIdUnit);
            string result = JsonConvert.SerializeObject(list);
            return Ok(result);
        }


        [HttpGet("GetPurchaseWorking")]
        public IActionResult GetPurchaseWorking(DateTime fromDate, DateTime toDate, string vchType, string grnNo, string locIdUnit)
        {
            DataTable list = _pur.GetPurchaseWorking(fromDate, toDate, vchType, grnNo, locIdUnit);
            string result = JsonConvert.SerializeObject(list);
            return Ok(result);
        }


        [HttpPost("SavePurchaseWorking")]
        public IActionResult SavePurchaseWorking(List<PurchaseWorkingVM> p)
        {
            bool data = _pur.SavePurchaseWorking(p);

            return Ok(data);
        }


        [HttpDelete("DelPurchaseWorking")]
        public IActionResult DelPurchaseWorking(int vchNo, string vchType, bool workDone)
        {
            var data = _pur.DelPurchaseWorking(vchNo, vchType, workDone);

            return Ok(data);
        }


        #endregion

        #region Purchase Contract

        [HttpGet("GetPurchaseContractVchNo")]
        public IActionResult GetPurchaseContractVchNo()
        {
            var data = _pur.GetPurchaseContractVchNo();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SavePurchaseContract")]
        public IActionResult SavePurchaseContract(List<PurchaseContractVM> p)
        {
            object data = _pur.SavePurchaseContract(p);
            return Ok(data);
        }

        [HttpGet("GetPurchaseContractList")]
        public IActionResult GetPurchaseContractList(DateTime fromDate, DateTime toDate)
        {
            var data = _pur.GetPurchaseContractList(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetEditPurchaseContract")]
        public IActionResult GetEditPurchaseContract(int vchNo)
        {
            var data = _pur.GetEditPurchaseContract(vchNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpDelete("DelPurchaseContract")]
        public IActionResult DelPurchaseContract(int vchNo)
        {
            var data = _pur.DelPurchaseContract(vchNo);

            return Ok(data);
        }

        #endregion

        #region Purchase Correction


        [HttpGet("GetMaxGpNo")]
        public IActionResult GetMaxGpNo()
        {
            var data = _pur.GetMaxGpNo();
            var result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        //NOT USED
        [HttpGet("GetPurchaseCorrectionVchNo")]
        public IActionResult GetPurchaseCorrectionVchNo()
        {
            var data = _pur.GetPurchaseCorrectionVchNo();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpGet("GetPurchaseCorrectionList")]
        public IActionResult GetPurchaseCorrectionList(DateTime fromDate, DateTime toDate)
        {
            var data = _pur.GetPurchaseCorrectionList(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetEditPurchaseCorrection")]
    
        public IActionResult GetEditPurchaseCorrection(int vchNo)
    {
        var data = _pur.GetEditPurchaseCorrection(vchNo);
        return Ok(data);
        }



        [HttpPost("SavePurchaseCorrection")]
        public IActionResult SavePurchaseCorrection(List<PurchaseCorrectionVM> p)
        {
            bool data = _pur.SavePurchaseCorrection(p);

            return Ok(data);
        }

        [HttpDelete("DelPurchaseCorrection")]
        public IActionResult DelPurchaseCorrection(int vchNo, string vchType)
        {
            var data = _pur.DelPurchaseCorrection(vchNo, vchType);

            return Ok(data);
        }


        #endregion

        #region Gate Pass Inward Entry (Purchase)

        [HttpGet("GetGatePassInwardEntryVchNo")]
        public IActionResult GetGatePassInwardEntryVchNo(string vchType)
        {
            var data = _gp.GetGatePassInwardEntryVchNo(vchType);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveGatePassInwardEntry")]
        public IActionResult SaveGatePassInwardEntry(List<PurchaseGatePassInwardEntryVM> gp)
        {
            object data = _gp.SaveGatePassInwardEntry(gp);

            return Ok(data);
        }

        [HttpGet("GetGatePassInwardList")]
        public IActionResult GetGatePassInwardList(DateTime fromDate, DateTime toDate, string vchType, string grnNo, string tag)
        {
            var data = _gp.GetGatePassInwardList(fromDate, toDate, vchType, grnNo, tag);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetEditGatePassInward")]
        public IActionResult GetEditGatePassInward(int vchNo, string vchType)
        {
            var data = _gp.GetEditGatePassInward(vchNo, vchType);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpDelete("DelGatePassInward")]
        public IActionResult DelGatePassInward(int vchNo, string vchType)
        {
            var data = _gp.DelGatePassInward(vchNo, vchType);

            return Ok(data);
        }

        [HttpGet("CheckVechicleGPInward")]
        public IActionResult CheckVechicleGPInward(string vehicalNo, int vchno, string vchtType, DateTime vchDate)
        {
            var data = _gp.CheckVechicleGPInward(vehicalNo, vchno, vchtType, vchDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region Lab

        [HttpGet("GetTestTypes")]
        public IActionResult GetTestTypes()
        {
            var data = _lab.GetTestTypes();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetArrivalList")]
        public IActionResult GetArrivalList()
        {
            var data = _lab.GetArrivalList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetLabNo")]
        public IActionResult GetLabNo()
        {
            var data = _lab.GetLabNo();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveLabResult")]
        public IActionResult SaveLabResult(List<LabVM> Lab)
        {
            var data = _lab.SaveLabResult(Lab);
            return Ok(data);
        }

        [HttpPost("SaveLabFirstSample")]
        public IActionResult SaveLabFirstSample(TblLabResult Lab)
        {
            var data = _lab.SaveLabFirstSample(Lab);
            return Ok(data);
        }

      
        [HttpGet("GetFirstLabList")]
        public IActionResult GetFirstLabList()
        {
            var data = _lab.GetFirstLabList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetLabsResultList")]
        public IActionResult GetLabsResultList(DateTime fromDate, DateTime toDate)
        {
            var data = _lab.GetLabsResultList(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetEditLab")]
        public IActionResult GetEditLab(int LabTestNo)
        {
            var data = _lab.GetEditLab(LabTestNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpDelete("DelLab")]
        public IActionResult DelLab(int LabTestNo)
        {
            bool data = _lab.DelLab(LabTestNo);
            return Ok(data);
        }

        // Lab Test Type


        [HttpGet("GetLabTestTypeList")]
        public IActionResult GetLabTestTypeList()
        {
            var data = _lab.GetLabTestTypeList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }



        [HttpPost("AddUpdateLabTestType")]
        public IActionResult AddUpdateLabTestType(int LabTestNo, string LabTestName, string TestUom)
        {
            var data = _lab.AddUpdateLabTestType(LabTestNo, LabTestName, TestUom);
            return Ok(data);
        }


        [HttpDelete("DeleteLabTestType")]
        public IActionResult DeleteLabTestType(int LabTestNo)
        {
            string data = _lab.DeleteLabTestType(LabTestNo);
            return Ok(data);
        }


        #endregion

        #region Miaze Rate
        [HttpGet("GetMaizeVchNo")] 
        public IActionResult GetMaizeVchNo()
        {
            var data = _pur.GetMaizeVchNo();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetMaizeItem")]

        public IActionResult GetMaizeItem()
        {
            var data = _pur.GetMaizeItem();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetMaizeRateList")]

        public IActionResult GetMaizeRateList()
        {
            var data = _pur.GetMaizeRateList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpPost("AddUpdateMaizeRate")]
        public IActionResult AddUpdateMaizeRate(int vchno, string itemCode, string Moisture, DateTime FromDate, DateTime ToDate, float Rate, string uom)
        {
            var data = _pur.AddUpdateMaizeRate(vchno, itemCode, Moisture, FromDate, ToDate, Rate, uom);
            return Ok(data);
        }

        [HttpDelete("DeleteMaizeRate")]
        public IActionResult DeleteMaizeRate(int VchNo)
        {
            var data = _pur.DeleteMaizeRate(VchNo);
            return Ok(data);
        }

        #endregion

    }
}