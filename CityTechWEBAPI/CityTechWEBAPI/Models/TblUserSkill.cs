using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblUserSkill
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SkillId { get; set; }

    public bool Active { get; set; }
}
