using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblMonthClose
{
    public int Id { get; set; }

    public int CompId { get; set; }

    public int? FinId { get; set; }

    public int? MonthClosingDate { get; set; }

    public DateTime? MonthOpening { get; set; }

    public DateTime? AutoClosingDate { get; set; }

    public bool? Closing { get; set; }
}
