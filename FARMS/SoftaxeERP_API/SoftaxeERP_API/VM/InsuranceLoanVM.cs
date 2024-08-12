namespace SoftaxeERP_API.VM
{
    public class InsuranceLoanVM
    {

        public int EmpyId { get; set; }

        public int Srno { get; set; }

        public DateTime? Stdate { get; set; }

        public int? Noofmnth { get; set; }

        public double? Loanamt { get; set; }

        public double? Instamt { get; set; }

        public string Remarks { get; set; }

        public bool? Active { get; set; }

        public bool? Sent { get; set; }

        public string Vehicleno { get; set; }

        public double? Opening { get; set; }

        public string Engineno { get; set; }

        public string Chasisno { get; set; }

    }
}
