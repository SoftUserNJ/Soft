using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblEobi
{
    public int CompId { get; set; }

    public int EmpyId { get; set; }

    public string LocId { get; set; }

    public int Srno { get; set; }

    public DateTime? Stdate { get; set; }

    public double? Eobi { get; set; }

    public string Remarks { get; set; }

    public string Userid { get; set; }

    public string Editby { get; set; }

    public int? Id { get; set; }

    public double? EobiDeducation { get; set; }

    public DateTime? Trdate { get; set; }

    public string Reference { get; set; }

    public bool? Sent { get; set; }

    public string Vch { get; set; }

    public bool? Active { get; set; }

    public int FinId { get; set; }
}
