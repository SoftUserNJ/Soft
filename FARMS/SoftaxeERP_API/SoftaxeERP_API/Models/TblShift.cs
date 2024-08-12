using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblShift
{
    public int CompId { get; set; }

    public int ShiftId { get; set; }

    public string ShiftName { get; set; }

    public string FromTime { get; set; }

    public string ToTime { get; set; }
}
