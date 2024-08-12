using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblIncidentWork
{
    public int IncidentNo { get; set; }

    public int WorkSno { get; set; }

    public string? WorkDes { get; set; }

    public string? WorkDetail { get; set; }

    public string? ImageBefore { get; set; }

    public string? ImageAfter { get; set; }

    public string? CustomerSign { get; set; }

    public string? MechanicSign { get; set; }

    public bool? FixedStatus { get; set; }

    public bool? Revisit { get; set; }

    public string? WorkStatus { get; set; }

    public DateTime? WorkDate { get; set; }

    public DateTime? WorkDateEnd { get; set; }

    public int? MechanicId { get; set; }

    public int? UserId { get; set; }

    public DateTime? ScheduleDate { get; set; }

    public string? Activity { get; set; }
}
