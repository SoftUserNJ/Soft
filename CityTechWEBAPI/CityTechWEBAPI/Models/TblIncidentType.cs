using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblIncidentType
{
    public int IncidentTypeId { get; set; }

    public string IncidentName { get; set; } = null!;

    public int SkillId { get; set; }

    public string Prepration { get; set; } = null!;

    public string Requirements { get; set; } = null!;

    public string PrioType { get; set; } = null!;

    public int Slaresponse { get; set; }

    public int Slasecure { get; set; }

    public int Slafixed { get; set; }
}
