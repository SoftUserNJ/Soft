using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblSlabRate
{
    public int Id { get; set; }

    public int? QtyFrom1 { get; set; }

    public int? QtyTo1 { get; set; }

    public double? Less1 { get; set; }

    public int? QtyFrom2 { get; set; }

    public int? QtyTo2 { get; set; }

    public double? Less2 { get; set; }

    public int? QtyFrom3 { get; set; }

    public double? Less3 { get; set; }

    public double? Whfiler { get; set; }

    public double? WhnonFiler { get; set; }
}
