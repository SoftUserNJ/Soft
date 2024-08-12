using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblObjectLocation
{
    public int LocId { get; set; }

    public string LocName { get; set; } = null!;

    public string? LocName2 { get; set; }

    public int StationId { get; set; }

    public int CustomerId { get; set; }

    public string? ContactPerson { get; set; }

    public string? ContactPersonPhone { get; set; }

    public string? Residence { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Lati { get; set; }

    public string? Longi { get; set; }
}
