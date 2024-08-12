namespace SoftaxeERP_API.VM
{
    public class OrderVM
    {
        public int DoNo { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public int DeliveryBoy { get; set; }
        public int OrderTaker { get; set; }
        public string ProductCode { get; set; }
        public string StockCode { get; set; }
        public string SMaxRate { get; set; }
        public double Rate { get; set; }
        public decimal DelQty { get; set; }
        public decimal SaleTax { get; set; }
        public decimal SaleTaxAmt { get; set; }
        public double NetBill { get; set; }
        public int InvNo { get; set; }
        public string LocId { get; set; }
        public string Status { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime VchDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Uom { get; set; }
        public int GID { get; set; }
        public int RID { get; set; }
        public int SID { get; set; }
        public string TermsDays { get; set; }
        public double FTax { get; set; }
        public double FTaxAmt { get; set; }
        public double WHT { get; set; }
        public DateTime DtNow { get; set; }
    }
}
