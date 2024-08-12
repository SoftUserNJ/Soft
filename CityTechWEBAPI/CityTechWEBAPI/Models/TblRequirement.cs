using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblRequirement
{
    public int Id { get; set; }

    public string Requirement { get; set; } = null!;
}
