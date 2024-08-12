namespace SoftaxeERP_API.VM
{
    public class SaleInvoiceVM
    {
        public int Dono { get; set; }
        public int InvNo { get; set; }
        public string VchType { get; set; }
        public DateTime VchDate { get; set; }
        public DateTime DueDate { get; set; }
        public string LocId { get; set; }
        public int DeliveryBoy { get; set; }
        public int OrderTaker { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string WalkingName { get; set; }
        public string WalkingContact { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int SaleQty { get; set; }
        public decimal RetQty { get; set; }
        public decimal NetQty { get; set; }
        public double Rate { get; set; }
        public double RateDiff { get; set; }
        public decimal ProductDis { get; set; }
        public decimal ProductDisAmt { get; set; }
        public decimal SaleTax { get; set; }
        public decimal SaleTaxAmt { get; set; }
        public double NetBill { get; set; }
        public string Uom { get; set; }
        public int GID { get; set; }
        public int RID { get; set; }
        public int SID { get; set; }
        public DateTime ExpDate { get; set; }
        public double OldRate { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmt { get; set; }

        public decimal OtherCredit { get; set; }
        public string Remarks { get; set; }
        public decimal Shipment { get; set; }
        public decimal RecAmount { get; set; }
        public double TotalNetBill { get; set; }
        public string BatchNo { get; set; }
        public string VehicleNo { get; set; }
        public double ReturnAmt { get; set; }
        public decimal TaxP { get; set; }
        public string Status { get; set; }
        public string Des { get; set; }
        public string TermsDays { get; set; }
        public double FTax { get; set; }
        public double FTaxAmt { get; set; }
        public double WHT { get; set; }
        public double WHTAmt { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodRmk { get; set; }
        public string SaleRemarks { get; set; }
        public int JobNo { get; set; }
        public string JobName { get; set; }
        public bool NoStock { get; set; }
        public string Tag { get; set; }
        public DateTime DtNow { get; set; }
    }
}
