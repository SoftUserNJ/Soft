using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class FarmShareHolder
{
    public int Id { get; set; }

    public int CmpId { get; set; }

    public string LocId { get; set; }

    public string Code { get; set; }

    public int? FarmId { get; set; }

    public double? Share { get; set; }

    public string ShareType { get; set; }
}
