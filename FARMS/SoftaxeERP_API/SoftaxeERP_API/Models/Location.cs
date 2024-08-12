using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Location
{
    public int Id { get; set; }

    public int CmpId { get; set; }

    public string LocId { get; set; }

    public string LocName { get; set; }

    public string LocType { get; set; }

    public string Cashcode { get; set; }

    public string CostCode { get; set; }

    public bool? MinWtasFinal { get; set; }

    public bool? PurOrderMust { get; set; }

    public bool? StopAutoAproveRp { get; set; }

    public bool? CreditLimitCheck { get; set; }

    public bool? AutoBillNo { get; set; }

    public string DayCloseTime { get; set; }

    public string CashCodeFac { get; set; }

    public string CashCodeHo { get; set; }

    public string BankCode { get; set; }

    public string CreditorsCode { get; set; }

    public string DebtorsFeedCode { get; set; }

    public string DebtorsWandaCode { get; set; }

    public string FeedsCode { get; set; }

    public string WandaCode { get; set; }

    public string FreightCode { get; set; }

    public string FreightCode1 { get; set; }

    public bool? GpoutSlip { get; set; }

    public int? GpoutSlipNos { get; set; }

    public int? InSlipNos { get; set; }

    public int? OutSlipNos { get; set; }

    public string VerifyByHead { get; set; }

    public string PrepByHead { get; set; }

    public string AppByHead { get; set; }

    public string AuditByHead { get; set; }

    public string RecByHead { get; set; }

    public string PrepByHead1 { get; set; }

    public string OtherHead1 { get; set; }

    public string AppByHead1 { get; set; }

    public string OtherHead2 { get; set; }

    public string OtherHead3 { get; set; }

    public string System { get; set; }

    public int? Sent { get; set; }

    public bool? AllowCoaho { get; set; }

    public bool? AllowCoafac { get; set; }

    public bool? Ghee { get; set; }

    public bool? IsTransporter { get; set; }

    public string Ntn { get; set; }

    public string Stn { get; set; }

    public bool? Dph { get; set; }

    public bool? Dpf { get; set; }

    public string Salestax { get; set; }

    public string BagsCode { get; set; }

    public bool? DayCloseSystem { get; set; }

    public string RefGoodsCode { get; set; }

    public string FgoodsCode { get; set; }

    public int? SaleRetChg { get; set; }

    public bool? RecipeSystem { get; set; }

    public string Cat1 { get; set; }

    public string Cat2 { get; set; }

    public string Cat3 { get; set; }

    public string Cat4 { get; set; }

    public int? VehOutPriority { get; set; }

    public string Plscode { get; set; }

    public string Explvl1Code { get; set; }

    public string SalesTaxCode { get; set; }

    public string FurtherTaxCode { get; set; }

    public string IncomeTaxCode { get; set; }

    public string SalesTaxCode1 { get; set; }

    public string FurtherTaxCode1 { get; set; }

    public string IncomeTaxCode1 { get; set; }

    public string IncomeLvl1Code { get; set; }

    public bool? AutoDayClose { get; set; }

    public int? MonthCloseAfter { get; set; }

    public bool? FacStockCloseSystem { get; set; }

    public string CmpName { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public string Contact { get; set; }

    public string Email { get; set; }
}
