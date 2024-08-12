namespace SoftaxeERP_API.VM
{
    public class SalarySettlementVM
    {
        public int EmpyId { get; set; }

        public int? deptId { get; set; }

        public int? desgnId { get; set; }

        public DateTime? entryDate { get; set; }

        public int typeId { get; set; }

        public DateTime? hireDate { get; set; }

        public DateTime? joinDate { get; set; }

        public int SrNo { get; set; }

        public string grade { get; set; }

        public int SalaryTypeId { get; set; }

        public int reason { get; set; }

        public double? grossSalary { get; set; }

        public double? Bsalary { get; set; }

        public double? lvl2 { get; set; }

        public double? lvl4 { get; set; }

        public double? lvl6 { get; set; }

        public double? lvl3 { get; set; }

        public double? lvl5 { get; set; }

        public double? lvl7 { get; set; }

        public string remarks { get; set; }

        public double? netSalary { get; set; }

        public double? Clvav { get; set; }

        public double? Mlvav { get; set; }

        public double? Elvav { get; set; }

        public string Empname { get; set; }

        public bool? active1 { get; set; }

        public double? bankSalary { get; set; }

        public double? cashSalary { get; set; }

    }
}
