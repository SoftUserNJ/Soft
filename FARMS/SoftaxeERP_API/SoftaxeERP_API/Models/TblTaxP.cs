using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblTaxP
{
    public int Id { get; set; }

    public int CompId { get; set; }

    public double? Tax { get; set; }

    public string Tag { get; set; }
}
