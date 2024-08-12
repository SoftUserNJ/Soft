namespace SoftaxeERP_API.VM
{
    public class StockOpVM
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public decimal Stock { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public int LocationId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UomId { get; set; }
        public string Uom { get; set; }
        public int JobNo { get; set; }
        public string JobName { get; set; }
        public DateTime DtNow { get; set; }

    }
}
