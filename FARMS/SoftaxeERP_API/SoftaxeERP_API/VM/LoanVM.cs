namespace SoftaxeERP_API.VM
{
    public class LoanVM
    {
       
        public int EmpyId { get; set; }

        public int SrNo { get; set; }

        public DateTime? startDate { get; set; }

        public int? Nom { get; set; }

        public double? Amount { get; set; }

        public double? mnthlyInstlment { get; set; }

        public string remarks { get; set; }

        public string Userid { get; set; }

        public string Editby { get; set; }

        public bool? tmpStop { get; set; }

        public string vchType { get; set; }

        public bool? Sent { get; set; }

        public string Type { get; set; }

    }
}
