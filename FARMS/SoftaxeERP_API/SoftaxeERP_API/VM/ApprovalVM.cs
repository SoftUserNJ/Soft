namespace SoftaxeERP_API.VM
{
    public class ApprovalVM
    {
        public int Voucher { get; set; }
        public string Reason { get; set; }
        public string VchType { get; set; }
        public int VchNo { get; set; }
        public string LocId { get; set; }
        public int FinId { get; set; }
        public string Tag { get; set; }
        public DateTime DtNow { get; set; }
    }
}
