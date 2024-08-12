using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblratediff
{
    public int? Vchno { get; set; }

    public string ItemCode { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public double? Rate { get; set; }

    public string Uom { get; set; }

    public double? AllowedWtdiff { get; set; }

    public int? FinId { get; set; }

    public string LocId { get; set; }

    public int Idd { get; set; }

    public int? CmpId { get; set; }

    public bool? Approve { get; set; }

    public string Moisture { get; set; }
}
