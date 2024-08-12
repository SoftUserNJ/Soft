namespace SoftaxeERP_API.VM
{
    public class VoucherTypeVM
    {
        public string VchType { get; set; }
        public bool StopEntry { get; set; }
        public bool CanVerify { get; set; }
        public bool CanUnVerify { get; set; }
        public bool CanApprove { get; set; }
        public bool CanUnApprove { get; set; }
        public bool CanAudit { get; set; }
        public bool CanUnAudit { get; set; }
        public int UId { get; set; }
    }
}
