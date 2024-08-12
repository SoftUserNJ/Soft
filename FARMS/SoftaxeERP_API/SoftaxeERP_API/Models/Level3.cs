using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Level3
{
    public string Level2 { get; set; }

    public string Level31 { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }

    public string Names { get; set; }

    public decimal? OpeningBal { get; set; }

    public int? Sent { get; set; }

    public string Tag { get; set; }

    public int? Uid { get; set; }

    public string Location { get; set; }

    public string MLoc { get; set; }

    public int? LocCount { get; set; }

    public bool? NotChange { get; set; }
}
