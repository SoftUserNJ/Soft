using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class TblLog
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public DateTime? LogDate { get; set; }

    public string? Activity { get; set; }

    public int? IncidentNo { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? Tag { get; set; }
}
