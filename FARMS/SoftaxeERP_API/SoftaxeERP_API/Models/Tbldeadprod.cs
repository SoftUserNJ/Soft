using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tbldeadprod
{
    public int? Id { get; set; }

    public int? Srno { get; set; }

    public int? Code { get; set; }

    public int? Qty { get; set; }

    public int? CompId { get; set; }

    public int? Finid { get; set; }

    public DateTime? Fromdate { get; set; }

    public DateTime? Todate { get; set; }
}
