using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblUserVchType
{
    public int Uid { get; set; }

    public string VchType { get; set; }

    public bool? CanFeed { get; set; }

    public bool? CanUnVerify { get; set; }

    public bool? CanUnApprove { get; set; }

    public bool? CanUnAudit { get; set; }

    public bool? CanVerify { get; set; }

    public bool? CanApprove { get; set; }

    public bool? CanAudit { get; set; }

    public int? Sent { get; set; }

    public int Idd { get; set; }
}
