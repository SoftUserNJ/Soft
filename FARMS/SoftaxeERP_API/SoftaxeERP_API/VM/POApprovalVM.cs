namespace SoftaxeERP_API.VM
{
    public class POApprovalVM
    {
        public int VchNo { get; set; }

        public bool IsApproved { get; set; }
        public bool IsVerified { get; set; }
        public bool IsAudited { get; set; }
        public bool IsPending { get; set; }
    }
}
