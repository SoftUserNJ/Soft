using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblcom
{
    public int? Id { get; set; }

    public int? Srno { get; set; }

    public double? Credit { get; set; }

    public double? Creditcotton { get; set; }

    public double? Cashcotton { get; set; }

    public double? Cash { get; set; }

    public DateTime? Fromdate { get; set; }

    public DateTime? Todate { get; set; }

    public int? CompId { get; set; }
}
