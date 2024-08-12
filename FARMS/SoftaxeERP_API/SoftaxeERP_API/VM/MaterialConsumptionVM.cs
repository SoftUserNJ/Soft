namespace SoftaxeERP_API.VM
{
    public class MaterialConsumptionVM
    {
        public int vchNo { get; set; }
        public string locationUnit { get; set; }
        public string date { get; set; }
        public string dmcode { get; set; }
        public string code { get; set; }
        public string prodLocation { get; set; }
        public string uom { get; set; }
        public float stock { get; set; }
        public float balance { get; set; }
        public float consQty { get; set; }
    }
}
