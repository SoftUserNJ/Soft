namespace SoftaxeERP_API.VM
{
    public class PurchaseContractVM
    {
        public string item { get; set; }
        public string category { get; set; }
        public float qty { get; set; }
        public string uom { get; set; }
        public float rate { get; set; }
        public float saleTax { get; set; }
        public int amount { get; set; }
        public string cropYear { get; set; }
        public int vehicle { get; set; }
        public int vchNo { get; set; }
        public DateTime date { get; set; }
        public DateTime poCompletionDate { get; set; }
        public string party { get; set; }
        public string broker { get; set; }
        public float incomeTax { get; set; }
        public int brockerCom { get; set; }
        public string brockerUom { get; set; }
        public string freightType { get; set; }
        public string bType { get; set; }
        public float bQty { get; set; }
        public int paymentDays { get; set; }
        public string invType { get; set; }
        public string remarks { get; set; }
        public string performaNo { get; set; }
        public DateTime performaDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public string insurance { get; set; }
        public DateTime coverDate { get; set; }
        public string hsCode { get; set; }
        public DateTime coreNoteDate { get; set; }

    }
}
