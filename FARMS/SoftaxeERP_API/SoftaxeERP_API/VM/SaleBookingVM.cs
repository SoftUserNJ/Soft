namespace SoftaxeERP_API.VM
{
    public class SaleBookingVM
    {
        public int VchNo { get; set; }
        public string VchType { get; set; }
        public DateTime Date { get; set; }
        public string CropYear { get; set; }
        public string DeliveryTerm { get; set; }
        public string PaymentTerm { get; set; }
        public string InvoiceType { get; set; }
        public string Broker { get; set; }
        public string Party { get; set; }
        public string Item { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public double BrokerComm { get; set; }
        public string BrokerUOM { get; set; }
        public string RateUom { get; set; }
        public string Remarks { get; set; }

    }
}
