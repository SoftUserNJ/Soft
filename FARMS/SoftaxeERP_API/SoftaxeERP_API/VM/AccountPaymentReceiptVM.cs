namespace SoftaxeERP_API.VM
{
    public class PRListVM
    {
        public List<PaymentReceiptVM> Payment { get; set; }
        public List<InvoiceListVM> Invoice { get; set; }
    }

    public class PaymentReceiptVM
    {
        public int VchNo { get; set; }
        public string VchType { get; set; }
        public DateTime VchDate { get; set; }
        public string BankCashName { get; set; }
        public string BankCash { get; set; }
        public string MainDesc { get; set; }
        public string AccountHead { get; set; }
        public string AccountHeadName { get; set; }
        public string ToAccountHead { get; set; }
        public string ToAccountHeadName { get; set; }
        public int JobNo { get; set; }
        public string JobName { get; set; }
        public string JobLocId { get; set; }
        public string Description { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }
        public string Tax1Name { get; set; }
        public double Tax1 { get; set; }
        public double Tax1Amount { get; set; }
        public string Tax2Name { get; set; }
        public double Tax2 { get; set; }
        public double Tax2Amount { get; set; }
        public double TotalTaxAmount { get; set; }
        public double NetAmount { get; set; }
        public string Status { get; set; }
        public string Module { get; set; }
        public DateTime DtNow { get; set; }
    }

    public class InvoiceListVM
    { 
        public int InvoiceNo { get; set; }
        public decimal RecAmount { get; set; }
        public string InvoiceType { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
