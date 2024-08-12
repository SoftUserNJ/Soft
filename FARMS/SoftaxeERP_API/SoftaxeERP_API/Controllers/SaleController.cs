using DevExpress.Pdf.Native.BouncyCastle.Ocsp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.Services;
using SoftaxeERP_API.VM;

namespace SoftaxeERP_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ICustomerSupplier _customer;
        private readonly IBillDueStatus _bill;
        private readonly ISaleByOrder _order;
        private readonly IPostDateCheque _pd;
        private readonly IServices _ser;
        private readonly IManagePOS _pos;
        private readonly ISaleInvoice _inv;
        private readonly IPurchaseInvoice _pur;
        private readonly IPaymentReceipts _pr;
        private readonly ICostCentre _cost;
        private readonly ICommonFieldsData _commonFields;
        private readonly ICurrency _currency;
        private readonly IDoEntry _doEntry;
        private readonly ISaleBooking _saleBooking;
        private readonly IGpOutWard _outWard;


        public SaleController(ICustomerSupplier customer, IBillDueStatus bill,
            ISaleByOrder order, IPostDateCheque pd, IServices ser, IManagePOS pos,
            ISaleInvoice inv, IPurchaseInvoice pur, IPaymentReceipts pr,
            ICostCentre cost, ICommonFieldsData commonFields, ICurrency currency, IDoEntry doEntry, ISaleBooking saleBooking, IGpOutWard outWard)
        {
            _customer = customer;
            _bill = bill;
            _order = order;
            _pd = pd;
            _ser = ser;
            _pos = pos;
            _inv = inv;
            _pur = pur;
            _pr = pr;
            _cost = cost;
            _commonFields = commonFields;
            _currency = currency;
            _doEntry = doEntry;
            _saleBooking = saleBooking;
            _outWard = outWard;
        }

        #region Sale Booking Entry


        [HttpGet("GetSaleBookingVchNo")]
        public IActionResult GetSaleBookingVchNo()
        {
            var data = _commonFields.GetMaxNo("BOOKINGNO", "Booking", "TBLBOOKING");
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetSaleBookingList")]
        public IActionResult GetSaleBookingList(DateTime fromDate, DateTime toDate)
        {
            var data = _saleBooking.GetSaleBookingList(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveSaleBookingEntry")]
        public IActionResult SaveSaleBookingEntry(List<SaleBookingVM> saleBooking)
        {
            var data = _saleBooking.SaveSaleBookingEntry(saleBooking);
            return Ok(data);
        }


        [HttpDelete("DelSaleBookingEntry")]
        public IActionResult DelSaleBookingEntry(int vchNo)
        {
            var data = _saleBooking.DelSaleBookingEntry(vchNo);
            return Ok(data);
        }

        [HttpGet("GetEditSaleBookingEntry")]
        public IActionResult GetEditSaleBookingEntry(int vchNo)
        {
            var data = _saleBooking.GetEditSaleBookingEntry(vchNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        #endregion

        #region Finished Goods Entry

        [HttpGet("GetCurrency")]
        public IActionResult GetCurrency()
        {
            var data = _currency.GetCurrency();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetPartyDisc")]
        public IActionResult GetPartyDisc(string code)
        {
            var data = _doEntry.GetPartyDisc(code);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpGet("GetDoProductList")]
        public IActionResult GetDoProductList(int categoryId, string productName, string barCode, DateTime invoiceDate, string vchType)
        {
            var data = _doEntry.GetProductList(categoryId, productName, barCode, invoiceDate, vchType);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetDoMaxNo")]
        public IActionResult GetDoMaxNo()
        {
            var data = _doEntry.GetDoMaxNo();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetDoList")]
        public IActionResult GetDoList(DateTime fromDate, DateTime toDate, string vchType)
        {
            var data = _doEntry.GetDoList(fromDate, toDate, vchType);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpPost("AddDO")]
        public IActionResult AddDO(List<DoSaleVM> doSale)
        {
            Boolean data = _doEntry.AddDO(doSale);
            return Ok(data);
        }

        [HttpGet("EditDo")]
        public IActionResult EditDo(int vchNo, string vchType)
        {
            var data = _doEntry.EditDo(vchNo, vchType);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpDelete("DeleteDo")]
        public IActionResult DeleteDo(int vchno, string vchType, DateTime dtNow)
        {
            var data = _doEntry.DeleteDo(vchno, vchType, dtNow);
            return Ok(data);
        }

        #endregion

        #region Sale Gate Out 


        [HttpGet("GetMaxVchNoGatePassOut")]
        public IActionResult GetMaxVchNoGatePassOut()
        {
            var data = _inv.GetMaxVchNoGatePassOut("vchno", "gp-out", "tbltransvch");
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpGet("GetDoListOutPass")]
        public IActionResult GetDoListOutPass(DateTime fromDate, DateTime toDate, string vchType, int vchNo)
        {
            var data = _inv.GetDoListOutPass(fromDate, toDate, vchType, vchNo);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetDoDetailListOutPass")]
        public IActionResult GetDoDetailListOutPass(DateTime doDate, string vchType, int vchNo)
        {
            var data = _inv.GetDoDetailListOutPass(doDate, vchType, vchNo);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveSaleGatePassOut")]
        public IActionResult SaveSaleGatePassOut(List<SaleGatePassOutVM> gp)
        {
            var data = _inv.SaveSaleGatePassOut(gp);
            return Ok(data);
        }

        #endregion

        #region MAIN AREA

        [HttpGet("GetMainArea")]
        public IActionResult GetMainArea()
        {
            var data = _customer.GetMainArea("Customer");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateMainArea")]
        public IActionResult AddUpdateMainArea(int id, string name, DateTime dtNow)
        {
            bool data = _customer.AddUpdateMainArea(id, name, "Customer", dtNow);

            return Ok(data);
        }

        [HttpDelete("DeleteMainArea")]
        public IActionResult DeleteMainArea(int id, DateTime dtNow)
        {
            var data = _customer.DeleteMainArea(id, "Customer", dtNow);

            return Ok(data);
        }

        #endregion

        #region SUB AREA

        [HttpGet("GetSubArea")]
        public IActionResult GetSubArea(int mainAreaId)
        {
            var data = _customer.GetSubArea(mainAreaId, "Customer");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateSubArea")]
        public IActionResult AddUpdateSubArea(int mainAreaId, int id, string name, DateTime dtNow)
        {
            bool data = _customer.AddUpdateSubArea(mainAreaId, id, name, "Customer", dtNow);

            return Ok(data);
        }

        [HttpDelete("DeleteSubArea")]
        public IActionResult DeleteSubArea(int mainAreaId, int id, DateTime dtNow)
        {
            var data = _customer.DeleteSubArea(mainAreaId, id, "Customer", dtNow);

            return Ok(data);
        }

        #endregion

        #region CUSTOMER

        [HttpGet("GetCustomer")]
        public IActionResult GetCustomer(bool status)
        {
            var data = _customer.GetCustomerSupplier("Customer", status);
            var result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateCustomer")]
        public IActionResult AddUpdateCustomer(CustomerSupplierVM customerSupplierVM)
        {
            string data = _customer.AddUpdateCustomerSupplier(customerSupplierVM);
            return Ok(data);
        }

        [HttpDelete("DeleteCustomer")]
        public IActionResult DeleteCustomer(string code, DateTime dtNow)
        {
            var data = _customer.DeleteCustomerSupplier(code, "Customer", dtNow);

            return Ok(data);
        }

        [HttpPost("CustomerTaxUpdate")]
        public IActionResult CustomerTaxUpdate(string vM)
        {
            List<CustomerTaxVM> list = JsonConvert.DeserializeObject<List<CustomerTaxVM>>(vM);
            bool data = _customer.CustomerTaxUpdate(list);
            return Ok(data);
        }

        #endregion

        #region CUSTOMER LEDGER

        [HttpGet("GetCustomerLedger")]
        public IActionResult GetSupplierLedger()
        {
            var data = _customer.GetLedgerList("D");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        #endregion

        #region SALE AREA UPDATION

        [HttpGet("SaleManagerList")]
        public IActionResult SaleManagerList()
        {
            var data = _order.SaleManagerList();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateSalesManager")]
        public IActionResult AddUpdateSalesManager(int id, string name, DateTime dtNow)
        {
            var data = _order.AddUpdateSalesManager(id, name, dtNow);
            return Ok(data);
        }

        [HttpDelete("DeleteSalesManager")]
        public IActionResult DeleteSalesManager(int id)
        {
            var data = _order.DeleteSalesManager(id);
            return Ok(data);
        }

        [HttpGet("GetAreaList")]
        public IActionResult GetAreaList()
        {
            var data = _order.GetAreaList();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateOTArea")]
        public IActionResult AddUpdateOTArea([FromBody] List<OTAreaVM> vM)
        {
            var data = _order.AddUpdateOTArea(vM);
            return Ok(data);
        }

        [HttpGet("GetOTAreas")]
        public IActionResult GetOTAreas(int id)
        {
            var data = _order.GetOTAreas(id);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetCommission")]
        public IActionResult GetCommission(int userId)
        {
            var data = _order.GetCommission(userId);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateCommission")]
        public IActionResult AddUpdateCommission(string recovery, string commission, string aboveCommission, string target, int userId)
        {
            var data = _order.AddUpdateCommission(recovery, commission, aboveCommission, target, userId);
            return Ok(data);
        }

        #endregion

        #region ORDER TAKER LIVE ACTIVITY

        [HttpGet("GetOrderTakerList")]
        public IActionResult GetOrderTakerList()
        {
            var data = _order.GetOrderTakerList();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetActivityList")]
        public IActionResult GetActivityList(int userId, string status, DateTime fromDate, DateTime toDate)
        {
            var data = _order.GetActivityList(userId, status, fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        #endregion

        #region DELIVERY PERSON

        [HttpGet("GetDeliveryPerson")]
        public IActionResult GetDeliveryPerson()
        {
            var data = _order.GetDeliveryPerson();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateDeliveryPerson")]
        public IActionResult AddUpdateDeliveryPerson(int id, string name, DateTime dtNow)
        {
            var result = _order.AddUpdateDeliveryPerson(id, name, dtNow);
            return Ok(result);
        }

        [HttpDelete("DeleteDeliveryPerson")]
        public IActionResult DeleteDeliveryPerson(int id, DateTime dtNow)
        {
            var result = _order.DeleteDeliveryPerson(id, dtNow);
            return Ok(result);
        }

        #endregion

        #region SALE BY ORDER

        [HttpGet("GetDoMax")]
        public IActionResult GetDoMax()
        {
            var data = _order.GetDoMax();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetOrderList")]
        public IActionResult GetOrderList(DateTime fromDate, DateTime toDate, string status)
        {
            var data = _order.GetOrderList(fromDate, toDate, status);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetOrderDetail")]
        public IActionResult GetOrderDetail(int dono)
        {
            var data = _order.GetOrderDetail(dono);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("CheckOrder")]
        public IActionResult CheckOrder(int doNo, int vchNo)
        {
            var data = _order.CheckOrder(doNo, vchNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("AddUpdateOrder")]
        public IActionResult AddUpdateOrder([FromBody] List<OrderVM> vM)
        {
            var data = _order.AddUpdateOrder(vM);
            return Ok(data);
        }

        [HttpPost("UpdateOrder")]
        public IActionResult UpdateOrder(DateTime vchDate, DateTime dueDate, int deliveryPerson, string terms, int invNo)
        {
            var data = _order.UpdateOrder(vchDate, dueDate, deliveryPerson, terms, invNo);
            return Ok(data);
        }

        [HttpDelete("DeleteInvoice")]
        public IActionResult DeleteInvoice(int vchno, string vchType, DateTime dtNow)
        {
            var data = _order.DeleteInvoice(vchno, vchType, dtNow);
            return Ok(data);
        }

        #endregion

        #region ORDER POSITION

        [HttpGet("GetOrderReceivedList")]
        public IActionResult GetOrderReceivedList(DateTime fromDate, DateTime toDate, string status)
        {
            var data = _order.GetOrderReceivedList(fromDate, toDate, status);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("OrderDelivered")]
        public IActionResult OrderDelivered(int doNo, int vchNo)
        {
            var result = _order.OrderDelivered(doNo, vchNo);
            return Ok(result);
        }

        #endregion

        #region BILL DUE STATUS

        [HttpGet("GetBillDueStatus")]
        public IActionResult GetBillDueStatus(DateTime fromDate, DateTime toDate, string status, int cmpId, string locId)
        {
            var data = _bill.SaleBillDueStatus(fromDate, toDate, status, cmpId, locId);
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

        #region PAYMENT / RECEIPTS

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
            var data = _pr.InvoiceList("sale", code);
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

        #region POST DATED CHEQUE

        [HttpGet("GetPDParty")]
        public IActionResult GetPDParty()
        {
            var data = _pd.GetPDParty("BR");
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
            var data = _pd.GetPDChequeList(fromDate, toDate, status, "BR");
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

        #region PROFIT MARGIN

        [HttpGet("GetOrderTakerBySM")]
        public IActionResult GetOrderTakerBySM(int saleManagerId)
        {
            var data = _order.GetOrderTakerBySM(saleManagerId);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        #endregion

        #region AGING

        [HttpGet("GetAging")]
        public IActionResult GetAging()
        {
            var data = _customer.GetAging("Customer");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdateAging")]
        public IActionResult SaveUpdateAging(int d1, int d2, int d3, int d4, int d5, int d6, int d7, DateTime dtNow)
        {
            var result = _customer.AddUpdateAging(d1, d2, d3, d4, d5, d6, d7, "Customer", dtNow);
            return Ok(result);
        }

        #endregion

        #region SALE INVOICE

        [HttpGet("GetProductList")]
        public IActionResult GetProductList(int categoryId, string productName, string barCode, DateTime invoiceDate, string vchType, string party)
        {
            var data = _inv.GetProductList(categoryId, productName, barCode, invoiceDate, vchType, party);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetInvoiceList")]
        public IActionResult GetInvoiceList(DateTime fromDate, DateTime toDate, string vchType)
        {
            var data = _inv.GetInvoiceList(fromDate, toDate, vchType);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveUpdateInvoice")]
        public async Task<IActionResult> SaveUpdateInvoice([FromBody] List<SaleInvoiceVM> invoice)
        {
           // List<SaleInvoiceVM> deserialObj = JsonConvert.DeserializeObject<List<SaleInvoiceVM>>(invoice);
            object result = await _inv.SaveUpdateInvoice(invoice);
            return Ok(result);
        }


        [HttpPost("SaveUpdateDeliveryOrder")]
        public async Task<IActionResult> SaveUpdateDeliveryOrder([FromBody] List<DispatchOrderVM> invoice)
        {
            object result = await _inv.SaveUpdateDeliveryOrder(invoice);
            return Ok(result);
        }

        [HttpGet("EditInvoice")]
        public IActionResult EditInvoice(int vchNo, string vchType, DateTime invoiceDate)
        {
            var data = _inv.EditInvoice(vchNo, vchType, invoiceDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        [HttpGet("GetVehicleNo")]
        public IActionResult GetVehicleNo()
        {
            var data = _inv.GetVehicleNo();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }



        #endregion  

        #region AVAIL SERVICE

        [HttpGet("GetServices")]
        public IActionResult GetServices()
        {
            var data = _ser.GetServices();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdateServices")]
        public IActionResult SaveUpdateServices(string code, string serviceName, int timePeriodId, double rate, double tax)
        {
            bool data = _ser.SaveUpdateServices(code, serviceName, timePeriodId, rate, tax);
            return Ok(data);
        }

        [HttpDelete("DeleteServices")]
        public IActionResult DeleteServices(string code)
        {
            var data = _ser.DeleteServices(code);
            return Ok(data);
        }

        [HttpGet("GetTimePeriod")]
        public IActionResult GetTimePeriod()
        {
            var data = _ser.GetTimePeriod();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdateTimePeriod")]
        public IActionResult SaveUpdateTimePeriod(int id, string name)
        {
            bool data = _ser.SaveUpdateTimePeriod(id, name);
            return Ok(data);
        }

        [HttpDelete("DeleteTimePeriod")]
        public IActionResult DeleteTimePeriod(int id)
        {
            var data = _ser.DeleteTimePeriod(id);
            return Ok(data);
        }


        [HttpGet("GetServiceProduct")]
        public IActionResult GetServiceProduct()
        {
            var data = _ser.GetServiceProduct();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdateServiceProduct")]
        public IActionResult SaveUpdateServiceProduct(int id, string name, double costRate, double saleRate)
        {
            bool data = _ser.SaveUpdateServiceProduct(id, name, costRate, saleRate);
            return Ok(data);
        }

        [HttpDelete("DelServiceProduct")]
        public IActionResult DelServiceProduct(int id)
        {
            var data = _ser.DelServiceProduct(id);
            return Ok(data);
        }

        [HttpGet("GetAvailService")]
        public IActionResult GetAvailService(string filterby, DateTime fromDate, DateTime toDate)
        {
            var data = _ser.GetAvailService(filterby, fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }


        [HttpGet("GetMaxService")]
        public IActionResult GetMaxService()
        {
            var data = _ser.GetMaxService();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveAvailService")]
        public IActionResult SaveAvailService(List<SevicesVM> vM)
        {
            bool data = _ser.SaveAvailService(vM);
            return Ok(data);
        }

        [HttpGet("EditService")]
        public IActionResult EditService(int id)
        {
            var data = _ser.EditService(id);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpDelete("DelAvailService")]
        public IActionResult DelAvailService(int id)
        {
            var data = _ser.DelAvailService(id);
            return Ok(data);
        }

        [HttpPost("MakeBillService")]
        public IActionResult MakeBillService(List<SevicesVM> vM)
        {
            var data = _ser.MakeBillService(vM);
            return Ok(data);
        }

        #endregion

        #region DAY CLOSING

        [HttpGet("GetDayClosingCash")]
        public IActionResult GetDayClosingCash()
        {
            var result = _pos.GetDayClosingCash();
            return Ok(result);
        }

        [HttpGet("GetDayClosingAccounts")]
        public IActionResult GetDayClosingAccounts(DateTime date, string shift, string till)
        {
            var data = _pos.GetDayClosingAccounts(date, shift, till);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveDayClosingAccounts")]
        public IActionResult SaveDayClosingAccounts(DateTime date, string paidTo, double cash)
        {
            var result = _pos.SaveDayClosingAccounts(date, paidTo, cash);
            return Ok(result);
        }

        [HttpGet("GetDayClosingAcc")]
        public IActionResult GetDayClosingAcc(DateTime date)
        {
            var result = _pos.GetDayClosingAcc(date);

            return Ok(result);
        }

        #endregion

        #region SALESMEN JOBS TODAY

        [HttpGet("GetSalesMenJobsFields")]
        public IActionResult GetSalesMenJobsFields()
        {
            return Ok(_pos.GetSalesMenJobsFields());
        }

        [HttpGet("GetSalesMan")]
        public IActionResult GetSalesMan(DateTime dateTime, int shift)
        {
            return Ok(_pos.GetSalesMan(dateTime, shift));
        }

        [HttpGet("GetSalesMenJobs")]
        public IActionResult GetSalesMenJobs()
        {
            var data = _pos.GetSalesMenJobs();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveSalesMenJobs")]
        public IActionResult SaveSalesMenJobs(int id, DateTime date, int shiftId, int tillId, int salesManId, string cashReceivedFrom, double cash, bool dayWise)
        {
            var result = _pos.SaveSalesMenJobs(id, date, shiftId, tillId, salesManId, cashReceivedFrom, cash, dayWise);
            return Ok(result);
        }

        [HttpDelete("DeleteSalesMenJobs")]
        public IActionResult DeleteSalesMenJobs(int id)
        {
            var result = _pos.DelSalesMenJobs(id);
            return Ok(result);
        }

        #endregion

        #region SALE ORDER

        [HttpGet("LoadBankCash")]
        public IActionResult LoadBankCash()
        {
            var data = _pur.LoadBankCash();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveBankCash")]
        public IActionResult SaveBankCash(string BankCask, string partyCode, DateTime VchDate, string TotalPayment, string Payment, string ChequeNo, DateTime ChequeDate, string Vchtype, int vchno, string status, int invNo, string invType)
        {
            var result = _pur.SaveBankCash(BankCask, partyCode, VchDate, TotalPayment, Payment, ChequeNo, ChequeDate, Vchtype, vchno, status, invNo, invType);

            return Ok(result);
        }

        [HttpDelete("DeletePayment")]
        public IActionResult DeletePayment(int Vchno, string Vchtype, string amount, int invoiceNo, string invType)
        {
            var result = _pur.DeletePayment(Vchno, Vchtype, amount, invoiceNo, invType);
            return Ok(result);
        }


        #endregion

        #region COST CENTRE

        [HttpGet("GetCostCentre")]
        public IActionResult GetCostCentre()
        {
            var data = _cost.GetCostCentre();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetAllCostCentre")]
        public IActionResult GetAllCostCentre()
        {
            var data = _cost.GetAllCostCentre();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveCostCentre")]
        public IActionResult SaveCostCentre(int id, string name, decimal commission, string comType, int rent, int rentInst, int userId)
        {
            var data = _cost.SaveCostCentre(id, name, commission, comType, rent, rentInst, userId);
            return Ok(data);
        }

        [HttpDelete("DeleteCostCentre")]
        public IActionResult DeleteCostCentre(int id)
        {
            var data = _cost.DeleteCostCentre(id);
            return Ok(data);
        }

        [HttpGet("GetShareHolderList")]
        public IActionResult GetShareHolderList()
        {
            var data = _cost.GetShareHolderList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        [HttpGet("GetShareHolders")]
        public IActionResult GetShareHolders(int id)
        {
            var data = _cost.GetShareHolders(id);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveShareHolder")]
        public IActionResult SaveShareHolder([FromBody] List<FarmShareHolder> farm)
        {
            var data = _cost.SaveShareHolder(farm);
            return Ok(data);
        }

        #endregion

        #region JOB NO

        [HttpGet("GetJobNoById")]
        public IActionResult GetJobNoById(int costCentreId)
        {
            var data = _cost.GetJobNoById(costCentreId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveJobNo")]
        public IActionResult SaveJobNo(int id, int jobNo, DateTime startDate, DateTime endDate, string remarks, int days, bool finished, int costCenterId, int totalChicks, int weight, int expense)
        {
            var data = _cost.SaveJobNo(id, jobNo, startDate, endDate, remarks, days, finished, costCenterId, totalChicks, weight, expense);
            return Ok(data);
        }

        [HttpDelete("DeleteJobNo")]
        public IActionResult DeleteJobNo(int id, int costCentreId)
        {
            string data = _cost.DeleteJobNo(id, costCentreId);
            return Ok(data);
        }

        #endregion

        #region SUBPARTY

        [HttpGet("GetSubPartyByCode")]
        public IActionResult GetSubPartyByCode(string code)
        {
            var data = _inv.GetSubPartyByCode(code);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveUpdateSubParty")]
        public IActionResult SaveUpdateSubParty(int id, string code, string name, DateTime dtNow)
        {
            var data = _inv.SaveUpdateSubParty(id, code, name, dtNow);
            return Ok(data);
        }

        [HttpDelete("DeleteSubParty")]
        public IActionResult DeleteSubParty(int id, DateTime dtNow)
        {
            var data = _inv.DeleteSubParty(id, dtNow);
            return Ok(data);
        }

        #endregion

        #region Finished Goods Production

        [HttpGet("GetFinishedGoodsProduction")]
        public IActionResult GetFinishedGoodsProduction(DateTime fromDate, DateTime toDate)
        {
            var data = _inv.GetFinishedGoodsProduction(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetEditFinishedGoodsProduction")]
        public IActionResult GetEditFinishedGoodsProduction(int vchNo)
        {
            var data = _inv.GetEditFinishedGoodsProduction(vchNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveFinishedGoodsProduction")]
        public IActionResult SaveFinishedGoodsProduction(List<FinishedGoodsProductionVM> fg)
        {
            bool data = _inv.SaveFinishedGoodsProduction(fg);
            return Ok(data);
        }

        [HttpDelete("DelFinishedGoodsProduction")]
        public IActionResult DelFinishedGoodsProduction(int vchNo)
        {
            bool data = _inv.DelFinishedGoodsProduction(vchNo);
            return Ok(data);
        }


        #endregion

        #region GATE PASS OUT WARD

        [HttpGet("GetGatePassList")]
        public IActionResult GetGatePassList()
        {
            var data = _outWard.GetGatePassList();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetMaxGpNo")]
        public IActionResult GetMaxGpNo()
        {
            var data = _outWard.GetMaxGpNo();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
            
        [HttpGet("GetPendingOrders")]
        public IActionResult GetPendingOrders()
        {
            var data = _outWard.GetPendingOrders();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetIOrderDetail")]
        public IActionResult GetIOrderDetail(int doNo)
        {
            var data = _outWard.GetIOrderDetail(doNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        #endregion

        [HttpGet("GetJobDueStatus")]
        public IActionResult GetJobDueStatus(DateTime fromDate, DateTime toDate)
        {
            var data = _bill.GetJobDueStatus(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        [HttpGet("GetJobNumber")]
        public IActionResult GetJobNumber(bool isFinished)
        {
            var data = _cost.JobList(isFinished);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetJobNumberByUser")]
        public IActionResult GetJobNumber(int userId)
        {
            var data = _cost.JobList(userId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("CostCentreStatus")]
        public IActionResult CostCentreStatus(DateTime fromDate, DateTime toDate)
        {
            var data = _cost.CostCentreStatus(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("PerformanceReport")]
        public IActionResult PerformanceReport(int jobNo)
        {
            var data = _cost.PerformanceReport(jobNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        

        [HttpGet("FarmExpReport")]
        public IActionResult FarmExpReport()
        {
            var data = _cost.FarmExpReport();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        

        [HttpGet("GetCostCodes")]
        public IActionResult GetCostCodes()
        {
            var data = _customer.GetCostCodes();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("OldPaymentList")]
        public IActionResult OldPaymentList(int invNo, string invType)
        {
            var data = _pur.OldPaymentList(invNo, invType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("ExpOrderNoUpdate")]
        public IActionResult ExpOrderNoUpdate(string vM)
        {
            List<ExpenseOrderVM> list = JsonConvert.DeserializeObject<List<ExpenseOrderVM>>(vM);
            bool data = _cost.ExpOrderNoUpdate(list);
            return Ok(data);
        }


        #region Party Terms and Condition

        [HttpPost("SaveTermsConditions")]
        public IActionResult SaveTermsConditions(List<TermConditionsVM> TC)
        {
            var data = _saleBooking.SaveTermsConditions(TC);
            return Ok(data);
        }


        [HttpGet("GetEditTermsConditions")]
        public IActionResult GetEditTermsConditions(int vchNo)
       {
            var data = _saleBooking.GetEditTermsConditions(vchNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpDelete("DelTermsConditions")]
        public IActionResult DelTermsConditions(int vchNo)
        {
            var data = _saleBooking.DelTermsConditions(vchNo);
            return Ok(data);
        }


        #endregion


        [HttpGet("GetDisCodes")]
        public IActionResult GetDisCodes()
        {
            var data = _inv.GetDisCodes();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetPartyTc")]
        public IActionResult GetPartyTc(string Party)
        {
            var data = _inv.GetPartyTc(Party);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        #region

        [HttpGet("GetMRRDetail")]
        public IActionResult GetMRRDetail(DateTime FromDate, DateTime toDate)
        {
            var data = _saleBooking.GetMRRDetail(FromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        [HttpPost("SentDoStatus")]
        public IActionResult SentDoStatus([FromBody] SentDoStatusRequest request)
        {
            var data = _inv.SentDoStatus(request);
            return Ok(data);
        }


        

        [HttpPost("UpdateDO")]
        public IActionResult UpdateDO(List<DOVM> vM)
        {
            var data = _outWard.UpdateDO(vM);
            return Ok(data);
        }

        [HttpPost("ClearGPNO")]
        public IActionResult ClearGPNO(ClearGpNoVM request)
        {
            var data = _outWard.ClearGPNO(request);
            return Ok(data);
        }

        [HttpPost("DeleteGPNO")]
        public IActionResult DeleteGPNO(ClearGpNoVM request)
        {
            var data = _outWard.DeleteGPNO(request);
            return Ok(data);
        }


        [HttpGet("GetEditGatePassOutward")]
        public IActionResult GetEditGatePassOutward(int GPNO)
        {
            var data = _outWard.GetEditGatePassOutward(GPNO);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }
    }
}