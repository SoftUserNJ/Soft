using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class TblObject
{
    public int ObjectId { get; set; }

    public string ObjectName { get; set; } = null!;

    public int CustomerId { get; set; }

    public int LocId { get; set; }

    public string? StationName { get; set; }

    public string? PostalCode { get; set; }
}
