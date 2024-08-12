using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblSecurity
{
    public int Id { get; set; }

    public string? MenuId { get; set; }

    public string? MenuName { get; set; }

    public string? Url { get; set; }

    public int? UserId { get; set; }

    public bool? Save { get; set; }

    public bool? Edit { get; set; }

    public bool? Delete { get; set; }

    public bool? Pdf { get; set; }

    public bool? Excel { get; set; }

    public bool? Word { get; set; }
}
