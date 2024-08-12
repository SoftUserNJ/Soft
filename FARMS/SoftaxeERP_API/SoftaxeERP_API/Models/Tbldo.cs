using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tbldo
{
    public int Dono { get; set; }

    public int Srno { get; set; }

    public DateTime? Dodate { get; set; }

    public int? Code { get; set; }

    public int? Mcode { get; set; }

    public int? Saleman { get; set; }

    public int? Ordertacker { get; set; }

    public string Location { get; set; }

    public double? Pcs { get; set; }

    public string Payment { get; set; }

    public double? Qty { get; set; }

    public double? Rate { get; set; }

    public double? Total { get; set; }

    public string Locid { get; set; }

    public int Finid { get; set; }

    public int Userid { get; set; }

    public int CompId { get; set; }

    public int? Groupid { get; set; }
}
