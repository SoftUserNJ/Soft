namespace SoftaxeERP_API.VM
{
    public class DailyConsumptionVM
    {
        public DateTime TransDate { get; set; }
        public int Week { get; set; }
        public float AvgWeight { get; set; }
        public float FeedConsumed { get; set; }
        public float Motality { get; set; }
        public float Diesel { get; set; }
        public string Remarks { get; set; }
        public int JobNo { get; set; }
        public DateTime Date { get; set; }
    }

}
