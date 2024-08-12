using System;
using System.Collections.Generic;

namespace MediaOutDoor.Models;

public partial class TblSetting
{
    public int Id { get; set; }

    public int? PlaySeconds { get; set; }

    public int? PlayTimes { get; set; }

    public string? PlayPer { get; set; }

    public string? RateB2b { get; set; }

    public string? RateB2c { get; set; }
}
