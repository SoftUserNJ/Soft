namespace SoftaxeERP_API.VM
{
    public class SevicesVM
    {
        public int TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime BillingDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public int MainAreaId { get; set; }
        public int SubAreaId { get; set; }
        public double TotalBill { get; set; }
        public double Discount { get; set; }
        public double DiscountAmount { get; set; }
        public string Remarks { get; set; }
        public int DueDateId { get; set; }
        public double TotalDue { get; set; }
        public double PaidAmount { get; set; }
        public double ReturnAmount { get; set; }
        public string PaymentMethod { get; set; }
        public double NetDue { get; set; }
        public int SPVoucher { get; set; }
        public string Status { get; set; }
        public int ProductNameId { get; set; }
        public string ProductName { get; set; }
        public decimal Qty { get; set; }
        public string Service { get; set; }
        public string ServiceCode { get; set; }
        public double CostRate { get; set; }
        public double ProductRate { get; set; }
        public decimal ProductTax { get; set; }
        public decimal ProductTaxAmount { get; set; }
        public double Total { get; set; }
        public string StockCode { get; set; }
        public string ProductRemarks { get; set; }
        public int GodownId { get; set; }
        public int RackId { get; set; }
        public int ShelId { get; set; }
        public int UomId { get; set; }
        public DateTime ExpDate { get; set; }
        public DateTime DtNow { get; set; }
    }
}
