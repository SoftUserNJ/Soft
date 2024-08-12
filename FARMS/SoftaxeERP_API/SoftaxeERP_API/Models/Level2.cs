using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Level2
{
    public string Level1 { get; set; }

    public string Level21 { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }

    public string Names { get; set; }

    public decimal? OpeningBal { get; set; }

    public float? Sent { get; set; }

    public int? Uid { get; set; }

    public string Location { get; set; }

    public string MLoc { get; set; }

    public int? LocCount { get; set; }

    public bool? NotChange { get; set; }
}
