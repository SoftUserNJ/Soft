namespace SoftaxeERP_API.VM
{
    public class ProductVM
    {
        public string SaleCode { get; set; }
        public string StockCode { get; set; }
        public string Code { get; set; }
        public int Category { get; set; }
        public int Brand { get; set; }
        public string Name { get; set; }
        public int UomId { get; set; }
        public int Packing { get; set; }
        public string ShortName { get; set; }
        public double SaleRate { get; set; }
        public double StandardWeight { get; set; }
        public double ItemWeight { get; set; }
        public double ItemPackedWeight { get; set; }
        public double Liter { get; set; }
        public double Discount { get; set; }
        public double SaleTax { get; set; }
        public int MadeIn { get; set; }
        public int Location { get; set; }
        public double MinimumLevel { get; set; }
        public string HSNo { get; set; }
        public bool Status { get; set; }
        public string BarCode { get; set; }
        public bool NoStock { get; set; }
        public decimal OldRate { get; set; }
        public decimal Rate2 { get; set; }
        public decimal Rate3 { get; set; }
        public double Rate4 { get; set; }
        public double Rate5 { get; set; }
        public double Rate6 { get; set; }
        public double Rate7 { get; set; }
        public int PurchaseRate1 { get; set; }
        public int PurchaseRate2 { get; set; }
        public IFormFile Picture { get; set; }
        public DateTime DtNow { get; set; }
    }
}
