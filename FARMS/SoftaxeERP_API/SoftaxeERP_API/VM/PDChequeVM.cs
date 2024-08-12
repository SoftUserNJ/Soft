namespace SoftaxeERP_API.VM
{
    public class PDChequeVM
    {
        public string Vchtype { get; set; }
        public int Vchno { get; set; }
        public bool Deposit { get; set; }
        public bool Cleared { get; set; }
        public bool Bounced { get; set; }
    }
}
