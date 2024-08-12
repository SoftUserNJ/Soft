using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblServiceProduct
{
    public int Id { get; set; }

    public string ProductName { get; set; }

    public int CompId { get; set; }

    public double? CostRate { get; set; }

    public double? SaleRate { get; set; }
}
