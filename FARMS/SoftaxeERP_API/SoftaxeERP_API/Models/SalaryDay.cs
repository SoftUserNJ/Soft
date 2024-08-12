using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class SalaryDay
{
    public int? SalaryDays { get; set; }

    public int Srno { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }
}
