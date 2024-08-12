using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblJob
{
    public int JobNo { get; set; }

    public int? CmpId { get; set; }

    public DateTime? DateTime { get; set; }

    public int? ShiftId { get; set; }

    public int? Till { get; set; }

    public int? SalesManId { get; set; }

    public int? FloorManagerId { get; set; }

    public double? Cash { get; set; }

    public string DmCode { get; set; }

    public string Code { get; set; }

    public int? VchNo { get; set; }

    public bool? DayWise { get; set; }
}
