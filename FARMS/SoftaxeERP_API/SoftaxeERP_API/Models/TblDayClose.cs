using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblDayClose
{
    public int Id { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }

    public int? FinId { get; set; }

    public DateTime? DayClose { get; set; }

    public bool? Closing { get; set; }
}
