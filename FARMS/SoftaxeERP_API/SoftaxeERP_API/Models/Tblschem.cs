using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblschem
{
    public int? Id { get; set; }

    public int? Srno { get; set; }

    public double? Cotton { get; set; }

    public double? Tcotton { get; set; }

    public string Pid { get; set; }

    public string Pid1 { get; set; }

    public double? Cottondis { get; set; }

    public DateTime? Fromdate { get; set; }

    public DateTime? Todate { get; set; }

    public int? CompId { get; set; }

    public string Type { get; set; }

    public string Payment { get; set; }

    public string Cate { get; set; }

    public string Remarks { get; set; }
}
