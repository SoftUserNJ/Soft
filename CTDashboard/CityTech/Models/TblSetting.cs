using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class TblSetting
{
    public string? Platform { get; set; }

    public int? CallerWait { get; set; }

    public int? EngineerCallerWait { get; set; }

    public string? Language { get; set; }

    public string? WebViewUrl { get; set; }

    public int? Automode { get; set; }
}
