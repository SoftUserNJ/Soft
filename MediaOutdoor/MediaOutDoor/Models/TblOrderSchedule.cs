using System;
using System.Collections.Generic;

namespace MediaOutDoor.Models;

public partial class TblOrderSchedule
{
    public int Id { get; set; }

    public string? VisitorId { get; set; }

    public DateTime? PlayDate { get; set; }

    public TimeSpan? SlotFrom { get; set; }

    public TimeSpan? SlotTo { get; set; }

    public int? NoOfSlots { get; set; }
}
