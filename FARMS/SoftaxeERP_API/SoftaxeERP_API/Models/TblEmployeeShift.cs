using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblEmployeeShift
{
    public int Id { get; set; }

    public string Shift { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }
}
