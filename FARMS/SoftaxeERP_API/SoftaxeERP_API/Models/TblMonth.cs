using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblMonth
{
    public int? Mnth { get; set; }

    public int? Year { get; set; }

    public int FinId { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }
}
