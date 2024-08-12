using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tbltradoffer
{
    public int Id { get; set; }

    public int Srno { get; set; }

    public double? Cotton { get; set; }

    public string Pid { get; set; }

    public double? Rupees { get; set; }

    public DateTime? Fromdate { get; set; }

    public DateTime? Todate { get; set; }

    public int CompId { get; set; }

    public string Payment { get; set; }

    public double? Fctn { get; set; }

    public double? Tctn { get; set; }

    public int? Groupid { get; set; }

    public string Type { get; set; }

    public string Vchtype { get; set; }
}
