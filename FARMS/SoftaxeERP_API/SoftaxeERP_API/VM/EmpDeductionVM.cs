namespace SoftaxeERP_API.VM
{
    public class EmpDeductionVM
    {

        public int EmpyId { get; set; }

        public string Vch { get; set; }
        public string Type { get; set; }

        public int Srno { get; set; }

        public DateTime? StDate { get; set; }

        public bool sent { get; set; }
        public string Reference { get; set; }

        public bool? Active { get; set; }

        public double? Amount { get; set; }

        public string Remarks { get; set; }

        public int? Id { get; set; }
    }
}
