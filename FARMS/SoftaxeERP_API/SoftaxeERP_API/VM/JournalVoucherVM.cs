namespace SoftaxeERP_API.VM
{
    public class JournalVoucherVM
    {
        public int VchNo { get; set; }
        public string VchType { get; set; }
        public DateTime VchDate { get; set; }
        public string Description { get; set; }
        public string Account { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public int Qty { get; set; }
        public int JobNo { get; set; }
        public string JobName { get; set; }
        public string Status { get; set; }
        public DateTime DtNow { get; set; }
    }
}
