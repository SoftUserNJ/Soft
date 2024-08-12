using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblCostCentre
{
    public int CostcentreId { get; set; }

    public string CostcentreName { get; set; }

    public string LocId { get; set; }

    public int CmpId { get; set; }

    public decimal? Comm { get; set; }

    public string ComType { get; set; }

    public int? Rent { get; set; }

    public int? RentInst { get; set; }

    public int? UserId { get; set; }
}
