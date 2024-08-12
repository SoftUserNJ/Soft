namespace SoftaxeERP_API.VM
{
    public class OvertimeVM
    {

        public int EmpyId { get; set; }

        public int Srno { get; set; }

        public DateTime? Stdate { get; set; }

        public string Remarks { get; set; }

        public int? Id { get; set; }

        public double OverTimeAmount { get; set; }


        public double? PerHourRate { get; set; }

        public double? TotalHrs { get; set; }

        public string LocId { get; set; }
    }
}
