using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblOverTime
{
    public int CompId { get; set; }

    public int EmpyId { get; set; }

    public int Srno { get; set; }

    public DateTime? Stdate { get; set; }

    public string Remarks { get; set; }

    public string Userid { get; set; }

    public string Editby { get; set; }

    public int? Id { get; set; }

    public double? OverTimeAmount { get; set; }

    public DateTime? Trdate { get; set; }

    public double? Ref { get; set; }

    public bool? Sent { get; set; }

    public double? PerHourRate { get; set; }

    public double? TotalHrs { get; set; }

    public bool? Active { get; set; }

    public string LocId { get; set; }

    public int FinId { get; set; }
}
