using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblPurchaseContractDetail
{
    public string VchType { get; set; }

    public int? PoNo { get; set; }

    public string Category { get; set; }

    public string Pcode { get; set; }

    public string PsubCode { get; set; }

    public string Icode { get; set; }

    public string IsubCode { get; set; }

    public DateTime? PoDate { get; set; }

    public DateTime? PoCompDate { get; set; }

    public DateTime? RecDate { get; set; }

    public string Bcode { get; set; }

    public string BsubCode { get; set; }

    public double? BrokerComm { get; set; }

    public string BrokerCommUom { get; set; }

    public string Remarks { get; set; }

    public int? Bags { get; set; }

    public double? Qty { get; set; }

    public double? Rate { get; set; }

    public double? ExRate { get; set; }

    public double? Drate { get; set; }

    public string ItemUom { get; set; }

    public double? ItemDivUom { get; set; }

    public int? NoOfVehicles { get; set; }

    public int? FinId { get; set; }

    public string LocId { get; set; }

    public int? Sno { get; set; }

    public int? Uid { get; set; }

    public bool? Verify { get; set; }

    public int? VerifyBy { get; set; }

    public bool? Aprove { get; set; }

    public int? AppBy { get; set; }

    public int? AuditBy { get; set; }

    public bool? AuditByN { get; set; }

    public bool? ControlOnBags { get; set; }

    public bool? ControlOnQty { get; set; }

    public bool? ControlOnVehicles { get; set; }

    public bool? Cancel { get; set; }

    public bool? CanceledBy { get; set; }

    public string InwardType { get; set; }

    public bool? Rcvd { get; set; }

    public double? OilContents { get; set; }

    public string Origon { get; set; }

    public string ContractNo { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string FreightType { get; set; }

    public int Idd { get; set; }

    public string BagsType { get; set; }

    public string InvoiceType { get; set; }

    public DateTime? EntryDate { get; set; }

    public double? BagsRate { get; set; }

    public int? SuppBrkrComsn { get; set; }

    public double? SaleTax { get; set; }

    public double? IncomeTax { get; set; }

    public int? Payafter { get; set; }

    public string CrpYear { get; set; }

    public int? CmpId { get; set; }

    public int? PoNetAmount { get; set; }

    public string Location { get; set; }
}
