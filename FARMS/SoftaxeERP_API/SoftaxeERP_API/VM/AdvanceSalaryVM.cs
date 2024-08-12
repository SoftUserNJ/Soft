namespace SoftaxeERP_API.VM
{
    public class AdvanceSalaryVM
    {
        public int? CompId { get; set; }

        public int EmpyId { get; set; }

        public int Srno { get; set; }

        public DateTime? Stdate { get; set; }

        public string Remarks { get; set; }

        public bool sent { get; set; }

        public double? AdvanceSalary { get; set; }

        public DateTime? Trdate { get; set; }

        public string Reference { get; set; }

        public string Vch { get; set; }

        public int? LocId { get; set; }
    }
}
