using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblottarget
{
    public int Id { get; set; }

    public double? Target1 { get; set; }

    public double? Rate1 { get; set; }

    public double? Target2 { get; set; }

    public double? Rate2 { get; set; }

    public double? Target3 { get; set; }

    public double? Rate3 { get; set; }

    public double? Target4 { get; set; }

    public double? Rate4 { get; set; }

    public int? Otid { get; set; }

    public int CompId { get; set; }

    public string Locid { get; set; }

    public int? Month { get; set; }
}
