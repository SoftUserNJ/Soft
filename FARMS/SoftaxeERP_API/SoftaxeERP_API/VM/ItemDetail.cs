namespace SoftaxeERP_API.VM
{
    public class ItemDetail
    {
        public int Sno { get; set; }
        public string Icode { get; set; }
        public string IsubCode { get; set; }
        public int Qty { get; set; }
        public int Rate { get; set; }
        public string ItemUom { get; set; }
        public int ItemDivUom { get; set; }
        public int NoOfVehicles { get; set; }
        public double SaleTax { get; set; }
    }
}
