using System;
using System.Collections.Generic;

namespace MediaOutdoor_Backend.Models;

public partial class TblScreen
{
    public int ScreenId { get; set; }

    public int StationId { get; set; }

    public string ScreenName { get; set; } = null!;

    public string ScreenSize { get; set; } = null!;

    public int? PlaySeconds { get; set; }

    public int? PlayTimes { get; set; }

    public string? PlayPer { get; set; }

    public string? Yposition { get; set; }

    public string? Xposition { get; set; }

    public string? Yposition1 { get; set; }

    public string? Xposition1 { get; set; }

    public string? Height { get; set; }

    public string? Width { get; set; }

    public double? Rate { get; set; }
}
