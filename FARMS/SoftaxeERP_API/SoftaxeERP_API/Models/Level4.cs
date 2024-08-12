using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Level4
{
    public string Level3 { get; set; }

    public string Level41 { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }

    public string Names { get; set; }

    public string Tag { get; set; }

    public string Tag1 { get; set; }

    public string ConsCode { get; set; }

    public string ConsNames { get; set; }

    public decimal? OpeningBal { get; set; }

    public decimal? OpeningBags { get; set; }

    public decimal? Openingqty { get; set; }

    public int? Sent { get; set; }

    public int? OrderId { get; set; }

    public int? Commission { get; set; }

    public int? Markup { get; set; }

    public int? Service { get; set; }

    public int? EditEntry { get; set; }

    public int? NewEntry { get; set; }

    public int? Uid { get; set; }

    public string Location { get; set; }

    public string MLoc { get; set; }

    public int? LocCount { get; set; }

    public string Vb6code { get; set; }

    public double? Opbal { get; set; }

    public string Tag2 { get; set; }

    public string MainCat { get; set; }

    public string Mappedcode { get; set; }

    public bool? NotChange { get; set; }
}
