using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class Tbldashboard
{
    public int Id { get; set; }

    public string? Tabid { get; set; }

    public string? TabContent { get; set; }

    public string? MainContent { get; set; }
}
