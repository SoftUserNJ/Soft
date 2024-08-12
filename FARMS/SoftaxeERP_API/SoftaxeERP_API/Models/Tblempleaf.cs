using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblempleaf
{
    public int EmpyId { get; set; }

    public int LvId { get; set; }

    public DateTime? Trdate { get; set; }

    public int? NoOfLvs { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }
}
