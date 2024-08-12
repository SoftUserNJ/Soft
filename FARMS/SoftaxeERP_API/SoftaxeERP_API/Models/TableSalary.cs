using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TableSalary
{
    public int? EmpyId { get; set; }

    public DateTime? BirthDate { get; set; }

    public string City { get; set; }

    public string Grade { get; set; }

    public DateTime? JoinDate { get; set; }

    public int? Through { get; set; }

    public string Ntn { get; set; }

    public int? DeptId { get; set; }

    public int? DesgId { get; set; }

    public double? Level2 { get; set; }

    public double? Level4 { get; set; }

    public double? Level6 { get; set; }

    public double? Level3 { get; set; }

    public double? Level5 { get; set; }

    public double? Level7 { get; set; }

    public double? Bsalary { get; set; }

    public double? Gsalary { get; set; }

    public double? Netsalary { get; set; }

    public double? Banksalary { get; set; }

    public double? Cashsalary { get; set; }

    public int? Location { get; set; }
}
