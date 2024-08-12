using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblProductsConversion
{
    public int Id { get; set; }

    public int CompId { get; set; }

    public string Locid { get; set; }

    public string Code { get; set; }

    public string Uom { get; set; }

    public double? Packing { get; set; }

    public double? PackingSize { get; set; }

    public double? PurchaseRate { get; set; }

    public double? MinRate { get; set; }

    public double? MaxRate { get; set; }
}
