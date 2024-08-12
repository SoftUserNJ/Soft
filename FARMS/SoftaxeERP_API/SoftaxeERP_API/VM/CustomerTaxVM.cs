namespace SoftaxeERP_API.VM
{
    public class CustomerTaxVM
    {
        public string Code { get; set; }
        public string SaleTax { get; set; }
        public string WHTax { get; set; }
        public DateTime DtNow { get; set; }
    }

    public class ExpenseOrderVM
    {
        public string Code { get; set; }
        public int OrderNo { get; set; }
    }
}
