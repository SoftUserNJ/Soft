using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblLvEnchasment
{
    public int CompId { get; set; }

    public int EmpyId { get; set; }

    public string LocId { get; set; }

    public int Srno { get; set; }

    public DateTime? Stdate { get; set; }

    public double? Grosssalary { get; set; }

    public double? Percentage { get; set; }

    public double? Lv { get; set; }

    public double? Lvpaid { get; set; }

    public double? Lvbalance { get; set; }

    public double? Bamount { get; set; }

    public double? Itax { get; set; }

    public double? Eobi { get; set; }

    public double? Pf { get; set; }

    public double? Loan { get; set; }

    public double? Vloan { get; set; }

    public double? Bonus { get; set; }

    public string Remarks { get; set; }

    public string Userid { get; set; }

    public string Editby { get; set; }

    public int? Id { get; set; }

    public DateTime? Trdate { get; set; }

    public string Reference { get; set; }

    public int FinId { get; set; }
}
