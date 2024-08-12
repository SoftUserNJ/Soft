using System;
using System.Collections.Generic;

namespace MediaOutdoor_Backend.Models;

public partial class TblSetting
{
    public int Id { get; set; }

    public int? PlaySeconds { get; set; }

    public int? PlayTimes { get; set; }

    public string? PlayPer { get; set; }

    public string? RateB2b { get; set; }

    public string? RateB2c { get; set; }

    public bool? ShowBudgetFrom { get; set; }

    public bool? ShowBudgetTo { get; set; }

    public bool? ShowDiscount { get; set; }

    public bool? ShowCpm { get; set; }

    public bool? ShowReach { get; set; }
}
