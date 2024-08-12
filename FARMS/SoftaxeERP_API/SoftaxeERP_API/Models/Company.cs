using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Company
{
    public int CmpId { get; set; }

    public int GrpId { get; set; }

    public DateTime? Date { get; set; }

    public string CmpName { get; set; }

    public string ShortName { get; set; }

    public string CmpAdr { get; set; }

    public string CmpCity { get; set; }

    public string Ntn { get; set; }

    public string OwnerName { get; set; }

    public string Country { get; set; }

    public string Contact { get; set; }

    public string Email { get; set; }

    public string Logo { get; set; }

    public string Currency { get; set; }

    public string CurrencySymbol { get; set; }

    public string DiscountCodeSale { get; set; }

    public string DiscountCodePurchase { get; set; }

    public string OtherCreditCodePurchase { get; set; }

    public string OtherCreditCodeSale { get; set; }

    public string ShipmentSaleCode { get; set; }

    public string ShipmentPurchaseCode { get; set; }

    public string CostofSale { get; set; }

    public string StkAdjustmentCode { get; set; }

    public string AccountOpningCode { get; set; }

    public string Tax1Code { get; set; }

    public string Tax2Code { get; set; }

    public string Whtcode { get; set; }

    public string FtaxCode { get; set; }

    public string OtherSaleTax { get; set; }

    public string InputSaleTax { get; set; }

    public string DistributionPos { get; set; }

    public string MobApp { get; set; }

    public string LocationWise { get; set; }

    public string Commission { get; set; }

    public string ChicksHead { get; set; }

    public string ChicksCode { get; set; }

    public string FeedHead { get; set; }

    public string FeedCode { get; set; }

    public string MediHead { get; set; }

    public string MediCode { get; set; }

    public string DieselHead { get; set; }

    public string DieselCode { get; set; }

    public string RentHead { get; set; }

    public string RentCode { get; set; }

    public string SalariesHead { get; set; }

    public string SalariesCode { get; set; }

    public string ElectricityHead { get; set; }

    public string ElectricityCode { get; set; }

    public string WoodHead { get; set; }

    public string WoodCode { get; set; }

    public string MessHead { get; set; }

    public string MessCode { get; set; }

    public string ShedEquipmentHead { get; set; }

    public string ShedEquipmentCode { get; set; }

    public string BagHead { get; set; }

    public string BagCode1 { get; set; }

    public string BagCode2 { get; set; }

    public string BagCode3 { get; set; }

    public string SaleHead { get; set; }

    public string SaleCode { get; set; }

    public double? FurtherTax { get; set; }

    public double? WhFiler { get; set; }

    public double? WhNonFiler { get; set; }

    public bool? ProductDiscountSale { get; set; }

    public bool? ProductDiscountPurchase { get; set; }

    public bool? TaxOnProduct { get; set; }

    public bool? PartyDiscountAllowed { get; set; }

    public bool? ApprovalSystem { get; set; }

    public bool? CommissionCustomer { get; set; }

    public bool? CommissionSupplier { get; set; }

    public bool? AppSys { get; set; }

    public bool? Ledger { get; set; }

    public bool? Aging { get; set; }

    public bool? Tax { get; set; }

    public bool? SaleRapCommission { get; set; }

    public bool? CreditLimit { get; set; }

    public bool? LoadParty { get; set; }

    public bool? ProductByCategory { get; set; }

    public bool? Stock { get; set; }

    public bool? StockExpiry { get; set; }

    public bool? Gl { get; set; }

    public bool? MonthClose { get; set; }

    public bool? DayClose { get; set; }

    public bool? Service { get; set; }

    public bool? RoundVal { get; set; }

    public int? ReportFormat { get; set; }

    public bool? CostCenterControl { get; set; }

    public bool? JobWiseControl { get; set; }

    public bool? BillWiseControl { get; set; }

    public bool? IsBroiler { get; set; }

    public bool? Islayers { get; set; }

    public bool? IsHatchery { get; set; }

    public string CashCode { get; set; }

    public string FreightPayableCode { get; set; }

    public bool? PoMust { get; set; }

    public string CashHo { get; set; }

    public string StkTransferCode { get; set; }

    public int? OutLimitOn { get; set; }

    public bool? ExportDetail { get; set; }
}
