﻿namespace SoftaxeERP_API.VM
{
    public class CompanyViewModel
    {
        public int GroupId { get; set; }
        public int CompId { get; set; }
        public DateTime Date { get; set; }
        public string CompanyName { get; set; }
        public string ShortName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Ntn { get; set; }
        public string StkAdj { get; set; }
        public string ShipmentSale { get; set; }
        public string ShipmentPurchase { get; set; }
        public string OtherCreditSale { get; set; }
        public string OtherCreditPurchase { get; set; }
        public string DiscountSale { get; set; }
        public string DiscountPurchase { get; set; }
        public string AccountOpening { get; set; }
        public string CostofSale { get; set; }
        public string Tax1 { get; set; }
        public string Tax2 { get; set; }
        public string FTax { get; set; }
        public string WhTax { get; set; }
        public string InputSaleTax { get; set; }
        public string OtherSaleTax { get; set; }
        public DateTime FinFromDate { get; set; }
        public DateTime FinToDate { get; set; }
        public string Currency { get; set; }
        public string Symbol { get; set; }
        public string Commission { get; set; }
        public string PosDistribution { get; set; }
        public string MobApp { get; set; }
        public string LocationWise { get; set; }
        public int? ReportFormat { get; set; }
        public int? FurtherTax { get; set; }
        public float WhFiler { get; set; }
        public float WhNonFiler { get; set; }
        public bool? Tax { get; set; }
        public bool? LedgerDetail { get; set; }
        public bool? SystemApproval { get; set; }
        public bool? DayClose { get; set; }
        public bool? MonthClose { get; set; }
        public bool? GL { get; set; }
        public bool? CommCustomer { get; set; }
        public bool? CommSupplier { get; set; }
        public bool? ProDisSale { get; set; }
        public bool? ProDisPurchase { get; set; }
        public bool? BillWiseControl { get; set; }
        public bool? CreditLimit { get; set; }
        public bool? PartyDisAlw { get; set; }
        public bool? TaxOnProduct { get; set; }
        public bool? SaleRapComm { get; set; }
        public bool? LoadParty { get; set; }
        public bool? Services { get; set; }
        public bool? ProByCategory { get; set; }
        public bool? Stock { get; set; }
        public bool? StockExpiry { get; set; }
        public bool? CostCenter { get; set; }
        public bool? JobWise { get; set; }
        public bool? Aging { get; set; }
        public bool? ExportDetails { get; set; }
        public bool? RoundVal { get; set; }
        public bool? IsBroiler { get; set; }
        public bool? Islayers { get; set; }
        public bool? IsHatchery { get; set; }
        public bool? PoMust { get; set; }
        public IFormFile Image { get; set; }
    }
}
