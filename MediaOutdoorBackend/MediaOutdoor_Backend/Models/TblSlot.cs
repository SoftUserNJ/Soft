using System;
using System.Collections.Generic;

namespace MediaOutdoor_Backend.Models;

public partial class TblSlot
{
    public int Id { get; set; }

    public TimeSpan FromTime { get; set; }

    public TimeSpan ToTime { get; set; }
}
