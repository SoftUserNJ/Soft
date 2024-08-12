namespace SoftaxeERP_API.VM
{
    public class SaleGatePassOutVM
    {
        public string doItem { get; set; }
        public int doQty { get; set; }
        public string doVchType { get; set; }
        public int dono { get; set; }
        public string doParty { get; set; }
        public string doDate { get; set; }
        public int vchNo { get; set; }
        public string vchType { get; set; }
        public DateTime vchDate { get; set; }
        public int extraFreight { get; set; }
        public int freightSubParty { get; set; }
        public int freight { get; set; }
        public string vehicleNo { get; set; }
        public string driverName { get; set; }
        public string driverContact { get; set; }
        public string driverCNIC { get; set; }
        public int biltyNo { get; set; }
        public int cropYear { get; set; }
        public int containerSize { get; set; }
        public string transporter { get; set; }
        public int fwdBooking1 { get; set; }
        public int fwdBooking2 { get; set; }
        public int country1 { get; set; }
        public int country2 { get; set; }
        public int port1 { get; set; }
        public int port2 { get; set; }
        public int containerNo1 { get; set; }
        public int containerNo2 { get; set; }
        public int sealNo1 { get; set; }
        public int sealNo2 { get; set; }
    }
}