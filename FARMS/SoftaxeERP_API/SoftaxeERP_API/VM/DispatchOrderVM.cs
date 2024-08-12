namespace SoftaxeERP_API.VM
{
    public class DispatchOrderVM
    {
        public int Dono { get; set; }
        public int InvNo { get; set; }
        public string VchType { get; set; }
        public DateTime VchDate { get; set; }
        public DateTime DueDate { get; set; }
        public string LocId { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string ProductSaleCode { get; set; }
        public string ProductStockCode { get; set; }
        public string ProductName { get; set; }
        public decimal SaleQty { get; set; }
        public double Rate { get; set; }
        public double RateDiff { get; set; }
        public decimal SaleTax { get; set; }
        public decimal SaleTaxAmt { get; set; }
        public string Uom { get; set; }
        public int GID { get; set; }
        public int RID { get; set; }
        public int SID { get; set; }
        public DateTime ExpDate { get; set; }
        public int Discount1 { get; set; }
        public double DiscountAmt1 { get; set; }

        public int Discount2 { get; set; }
        public double DiscountAmt2 { get; set; }

        public int Discount3 { get; set; }
        public double DiscountAmt3 { get; set; }

        public int Discount4 { get; set; }
        public double DiscountAmt4 { get; set; }

        public int Discount5 { get; set; }
        public double DiscountAmt5 { get; set; }

        public int Discount6 { get; set; }
        public double DiscountAmt6 { get; set; }
        public string Remarks { get; set; }
        public double NetDue { get; set; }
        public string VehicleNo { get; set; }
        public string Status { get; set; }
        public string Des { get; set; }
        public string TermsDays { get; set; }
        public double FTax { get; set; }
        public double FTaxAmt { get; set; }
        public double WHT { get; set; }
        public double WHTAmt { get; set; }
        public string SaleRemarks { get; set; }
        public int SubPartyId { get; set; }
        public int JobNo { get; set; }
        public string JobName { get; set; }
        public bool NoStock { get; set; }
        public DateTime DtNow { get; set; }
        public double NetValue { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NetRate { get; set; }
        public decimal Value { get; set; }
        public string DES1CODE { get; set; }
        public string DES2CODE { get; set; }
        public string DES3CODE { get; set; }
        public string DES4CODE { get; set; }
        public string DES5CODE { get; set; }
        public string DES6CODE { get; set; }

        public double totalQty { get; set;}

    }
}
