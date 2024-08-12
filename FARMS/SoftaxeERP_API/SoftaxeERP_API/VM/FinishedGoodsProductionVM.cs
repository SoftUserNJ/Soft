using DevExpress.CodeParser;

namespace SoftaxeERP_API.VM
{
    public class FinishedGoodsProductionVM
    {
        public int vchNo { get; set; }
        public string locationUnit { get; set; }
        public string date { get; set; }
        public string dmcode { get; set; }
        public string code { get; set; }
        public string prodLocation { get; set; }
        public string uom { get; set; }
        public int qty { get; set; }
        public float wt { get; set; }
    }
}
