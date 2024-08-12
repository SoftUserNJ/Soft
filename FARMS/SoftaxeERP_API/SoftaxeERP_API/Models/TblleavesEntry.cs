using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblleavesEntry
{
    public int CompId { get; set; }

    public int EmpyId { get; set; }

    public int Srno { get; set; }

    public DateTime? Stdate { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? EndDate { get; set; }

    public int? LvId { get; set; }

    public string Status { get; set; }

    public int? Nod { get; set; }

    public int? TotalLeaves { get; set; }

    public string Userid { get; set; }

    public decimal Id { get; set; }

    public string Editby { get; set; }

    public string LocId { get; set; }

    public string Remarks { get; set; }

    public string Vch { get; set; }

    public bool? Sent { get; set; }

    public int FinId { get; set; }
}
