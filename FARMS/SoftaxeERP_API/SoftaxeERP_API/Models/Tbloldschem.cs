using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tbloldschem
{
    public int? Id { get; set; }

    public int? Srno { get; set; }

    public int? Groupid { get; set; }

    public int? Code { get; set; }

    public int? Schemid { get; set; }

    public DateTime? Fromdate { get; set; }

    public DateTime? Todate { get; set; }

    public double? Cotton { get; set; }

    public int? CompId { get; set; }

    public int? Finid { get; set; }
}
