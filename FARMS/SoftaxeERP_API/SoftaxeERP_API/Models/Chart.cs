using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Chart
{
    public string Parent { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string Stat { get; set; }

    public string Gd { get; set; }

    public string Tag { get; set; }

    public double? OpenDr { get; set; }

    public double? OpenCr { get; set; }

    public string FullName { get; set; }

    public string ContactNo { get; set; }

    public string MobileNo { get; set; }

    public string Address { get; set; }

    public string Nic { get; set; }

    public string Area { get; set; }

    public string Sarea { get; set; }

    public string Salesman { get; set; }

    public string Pp { get; set; }

    public string Cost { get; set; }

    public string Price { get; set; }

    public double? OpenQty { get; set; }

    public string Ntn { get; set; }

    public string Stn { get; set; }

    public double? Pack { get; set; }

    public double? DueDays { get; set; }

    public double? OpenVal { get; set; }

    public string UnReg { get; set; }

    public string ConsCode { get; set; }

    public string Whtax { get; set; }
}
