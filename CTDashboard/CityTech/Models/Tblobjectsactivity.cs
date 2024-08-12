using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class Tblobjectsactivity
{
    public long Id { get; set; }

    public int? Objectid { get; set; }

    public DateTime? Date { get; set; }

    public double? Value { get; set; }

    public string? Tag { get; set; }
}
