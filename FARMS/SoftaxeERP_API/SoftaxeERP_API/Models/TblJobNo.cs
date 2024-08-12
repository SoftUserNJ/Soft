using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblJobNo
{
    public int JobNo { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Remarks { get; set; }

    public int CostcentreId { get; set; }

    public int CmpId { get; set; }

    public string LocId { get; set; }

    public int Id { get; set; }

    public bool? Finished { get; set; }

    public int? Days { get; set; }

    public int? TotalChicks { get; set; }

    public int? Weight { get; set; }

    public int? Expense { get; set; }
}
