namespace SoftaxeERP_API.VM
{
    public class ProductRateUpdateVM
    {
        public string Code { get; set; }
        public double SaleRate { get; set; }
        public int FromQty { get; set; }
        public int SlabRate { get; set; }
        public int ToQty { get; set; }
        public int AboveSlab { get; set; }
        public DateTime DtNow { get; set; }
    }
}
