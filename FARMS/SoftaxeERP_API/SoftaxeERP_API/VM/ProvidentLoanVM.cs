namespace SoftaxeERP_API.VM
{
    public class ProvidentLoanVM
    {

        public int EmpyId { get; set; }

        public int Srno { get; set; }

        public DateTime? Stdate { get; set; }

        public int? Noofmnth { get; set; }

        public double? Loanamt { get; set; }

        public double? Instamt { get; set; }

        public string Remarks { get; set; }

        public int? Id { get; set; }

        public bool? Active { get; set; }

        public bool? Sent { get; set; }
    }
}
