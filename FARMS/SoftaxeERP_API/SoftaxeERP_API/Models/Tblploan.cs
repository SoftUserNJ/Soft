using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblploan
{
    public int CompId { get; set; }

    public int EmpyId { get; set; }

    public string LocId { get; set; }

    public int Srno { get; set; }

    public DateTime? Stdate { get; set; }

    public int? Noofmnth { get; set; }

    public double? Loanamt { get; set; }

    public double? Instamt { get; set; }

    public string Remarks { get; set; }

    public string Userid { get; set; }

    public string Editby { get; set; }

    public int? Id { get; set; }

    public bool? Active { get; set; }

    public string Vch { get; set; }

    public bool? Sent { get; set; }

    public bool? FinEntry { get; set; }

    public string AccountCode { get; set; }

    public int FinId { get; set; }
}
