namespace SoftaxeERP_API.VM
{

    public class PurchaseVM
    {
        public List<PurchaseInvoiceVM> Purchase { get; set; }
        public List<DivisionVM> Division{ get; set; }
    }

    public class PurchaseInvoiceVM
    {
        public int InvNo { get; set; }
        public string VchType { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string ProductCode { get; set; }
        public string BatchNo { get; set; }
        public string Remarks { get; set; }
        public decimal Discount { get; set; }
        public double OtherCredit { get; set; }
        public double DiscountAmt { get; set; }
        public double RecAmount { get; set; }
        public double ReturnAmount { get; set; }
        public decimal RetQty { get; set; }
        public double SMinRate { get; set; }
        public double SMaxRate { get; set; }
        public double Rate { get; set; }
        public double PurchaseRate { get; set; }
        public decimal ProductDis { get; set; }
        public decimal ProductDisAmt { get; set; }
        public decimal SaleTax { get; set; }
        public decimal SaleTaxAmt { get; set; }
        public double NetBill { get; set; }
        public double TotalNetBill { get; set; }
        public double OtherAmount { get; set; }
        public string Status { get; set; }
        public DateTime ExpDate { get; set; }
        public DateTime VchDate { get; set; }
        public DateTime DueDate { get; set; }
        public int TotalQty { get; set; }
        public string Des { get; set; }
        public double Shipment { get; set; }
        public double OtherShipment { get; set; }
        public int NetQty { get; set; }
        public string Uom { get; set; }
        public string L5uom { get; set; }
        public int SID { get; set; }
        public string TermsDays { get; set; }
        public double FTax { get; set; }
        public double FTaxAmt { get; set; }
        public double WHT { get; set; }
        public double WHTAmt { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodRmk { get; set; }
        public DateTime DtNow { get; set; }
    }

    public class DivisionVM
    {
        public string CostCategoryCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal Qty { get; set; }
        public double Rate { get; set; }
        public double Value { get; set; }
        public int Commission { get; set; }
        public double CommissionAmt { get; set; }
        public double NetValue { get; set; }
        public string JobLocId { get; set; }
        public DateTime ExpDate { get; set; }
        public int SID { get; set; }
        public int JobNo { get; set; }
        public string JobName { get; set; }
    }
}
