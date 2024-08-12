using System;
using System.Collections.Generic;

namespace MediaOutdoor_Backend.Models;

public partial class TblPromotion
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public DateTime? Date { get; set; }

    public string? Status { get; set; }

    public DateTime? LastSentDate { get; set; }

    public bool? App { get; set; }

    public bool? Email { get; set; }

    public DateTime? ScheduleTime { get; set; }
}
