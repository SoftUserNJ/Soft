using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblhrsetup
{
    public string Type { get; set; }

    public string Name { get; set; }

    public string Category { get; set; }

    public int CmpId { get; set; }

    public string LocId { get; set; }

    public double? Amount { get; set; }

    public int HrSetupId { get; set; }
}
