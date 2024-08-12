using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblFlock
{
    public int Flockid { get; set; }

    public int FarmCode { get; set; }

    public string Flockno { get; set; }

    public int CompId { get; set; }
}
