using System;
using System.Collections.Generic;

namespace MediaOutdoor_Backend.Models;

public partial class TblCart
{
    public int CartId { get; set; }

    public string? VisitorId { get; set; }

    public int? ScreenId { get; set; }

    public DateTime? Date { get; set; }

    public int? StationId { get; set; }
}
