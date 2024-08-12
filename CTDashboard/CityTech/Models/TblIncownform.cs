using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class TblIncownform
{
    public int Id { get; set; }

    public int IncidentNo { get; set; }

    public int Formid { get; set; }

    public string? FormName { get; set; }

    public string? FormData { get; set; }

    public bool? IsMandatory { get; set; }

    public int? Count { get; set; }

    public bool? IsSaved { get; set; }
}
