using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblinsentive
{
    public int EmpyId { get; set; }

    public int Srno { get; set; }

    public DateTime? Trdate { get; set; }

    public double? Tel { get; set; }

    public double? Pet { get; set; }

    public double? Tada { get; set; }

    public double? Nightstay { get; set; }

    public double? Maint { get; set; }

    public double? Other { get; set; }

    public string Remarks { get; set; }

    public string Userid { get; set; }

    public string Editby { get; set; }

    public int? Id { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }

    public double? Total { get; set; }

    public double? Gym { get; set; }

    public double? House { get; set; }

    public double? Medical { get; set; }

    public double? Family { get; set; }

    public double? Security { get; set; }

    public int FinId { get; set; }
}
