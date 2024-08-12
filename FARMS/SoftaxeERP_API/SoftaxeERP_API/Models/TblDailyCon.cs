using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblDailyCon
{
    public int? JobNo { get; set; }

    public DateTime? VchDate { get; set; }

    public DateTime? TransDate { get; set; }

    public int? WeekNo { get; set; }

    public double? AvgWeight { get; set; }

    public double? Motality { get; set; }

    public double? FeedConsumed { get; set; }

    public double? DieselConsumed { get; set; }

    public string Remarks { get; set; }

    public int CmpId { get; set; }

    public string LocId { get; set; }

    public int? TotalChicks { get; set; }

    public double? Weight { get; set; }

    public int Id { get; set; }
}
