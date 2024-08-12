using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TransMain
{
    public int CmpId { get; set; }

    public string LocId { get; set; }

    public int FinId { get; set; }

    public string VchType { get; set; }

    public int VchNo { get; set; }

    public int? VchMonth { get; set; }

    public DateTime? VchDateM { get; set; }

    public int? Disc1 { get; set; }

    public int? Disc2 { get; set; }

    public int? Disc3 { get; set; }

    public int? Disc4 { get; set; }

    public int? Disc5 { get; set; }

    public int? Disc6 { get; set; }

    public int? Disc7 { get; set; }

    public int? Salestax { get; set; }

    public int? Tradeoffer { get; set; }

    public int? Sent { get; set; }

    public bool? Aprove { get; set; }

    public int? AppBy { get; set; }

    public int? Verify { get; set; }

    public int? VerifyBy { get; set; }

    public int? AuditBy { get; set; }

    public int? AuditByN { get; set; }

    public int? Printed { get; set; }

    public string Apploc { get; set; }

    public int? SendOnline { get; set; }

    public int? Med { get; set; }

    public string Status { get; set; }

    public int? FurtherTax { get; set; }

    public int? IncomeTax { get; set; }

    public double? IncomeTaxRate { get; set; }

    public string CustomerName { get; set; }

    public string CustomerContact { get; set; }

    public int? Firstwtby { get; set; }

    public int? Secwtby { get; set; }

    public bool? Gpapprove { get; set; }

    public int? GpapproveBy { get; set; }

    public double? Disc1Amt { get; set; }

    public double? Disc2Amt { get; set; }

    public double? Disc3Amt { get; set; }

    public double? Disc4Amt { get; set; }

    public double? Disc5Amt { get; set; }

    public double? Disc6Amt { get; set; }
}
