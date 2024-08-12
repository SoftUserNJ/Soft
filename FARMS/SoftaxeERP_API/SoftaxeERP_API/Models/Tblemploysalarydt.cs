using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblemploysalarydt
{
    public int CompId { get; set; }

    public string LocId { get; set; }

    public int EmpyId { get; set; }

    public int? DeptId { get; set; }

    public int? DesgId { get; set; }

    public DateTime? Trdate { get; set; }

    public int? EmpyType { get; set; }

    public DateTime? HireDate { get; set; }

    public DateTime? JoinDate { get; set; }

    public int Srno { get; set; }

    public string Grade { get; set; }

    public int? Through { get; set; }

    public int? Reasons { get; set; }

    public double? Gsalary { get; set; }

    public double? Bsalary { get; set; }

    public double? Level2 { get; set; }

    public double? Level4 { get; set; }

    public double? Level6 { get; set; }

    public double? Level3 { get; set; }

    public double? Level5 { get; set; }

    public double? Level7 { get; set; }

    public string Remarks { get; set; }

    public double? Netsalary { get; set; }

    public double? Clvav { get; set; }

    public double? Mlvav { get; set; }

    public double? Elvav { get; set; }

    public string Empname { get; set; }

    public bool? Active { get; set; }

    public double? Banksalary { get; set; }

    public double? Cashsalary { get; set; }
}
