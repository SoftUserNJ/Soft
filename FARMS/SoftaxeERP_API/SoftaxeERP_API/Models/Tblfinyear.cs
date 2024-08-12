using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblfinyear
{
    public int Finid { get; set; }

    public DateTime? Fromdate { get; set; }

    public DateTime? Todate { get; set; }

    public int Srno { get; set; }

    public int CompId { get; set; }
}
