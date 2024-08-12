using System;
using System.Collections.Generic;

namespace MediaOutDoor.Models;

public partial class TblScreen
{
    public int ScreenId { get; set; }

    public int StationId { get; set; }

    public string ScreenName { get; set; } = null!;

    public string ScreenSize { get; set; } = null!;

    public string? TopPosition { get; set; }

    public string? LeftPosition { get; set; }

    public double? Rate { get; set; }

    public string? Height { get; set; }

    public string? Width { get; set; }
}
