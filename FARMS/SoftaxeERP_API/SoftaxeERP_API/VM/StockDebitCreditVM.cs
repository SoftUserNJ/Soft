namespace SoftaxeERP_API.VM
{
    public class StockDebitCreditVM
    {
        public int VchNo { get; set; }
        public DateTime VchDate { get; set; }
        public string VchType { get; set; }
        public string ProductCode { get; set; }
        public int LocationId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UomId { get; set; }
        public decimal Qty { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string Tag { get; set; }
        public int JobNo { get; set; }
        public DateTime DtNow { get; set; }
    }
}
