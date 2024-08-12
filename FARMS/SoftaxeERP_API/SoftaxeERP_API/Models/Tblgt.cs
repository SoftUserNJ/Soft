using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblgt
{
    public int Id { get; set; }

    public double? Target { get; set; }

    public int CompId { get; set; }

    public string Locid { get; set; }

    public int? Month { get; set; }

    public int? Slot { get; set; }
}
