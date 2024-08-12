namespace SoftaxeERP_API.VM
{
    public class TransferVM
    {
        public int VchNo { get; set; }
        public DateTime VchDate { get; set; }
        public string ProductCode { get; set; }
        public int LocFromId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UomId { get; set; }
        public decimal Qty { get; set; }
        public int LocToId { get; set; }
        public string Status { get; set; }
        public int FromJobNo { get; set; }
        public string FromJobName { get; set; }
        public int ToJobNo { get; set; }
        public string ToJobName { get; set; }
        public string Tag { get; set; }
        public DateTime DtNow { get; set; }
    }
}
