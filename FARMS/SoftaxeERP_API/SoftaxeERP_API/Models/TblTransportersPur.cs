using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblTransportersPur
{
    public int? Id { get; set; }

    public string TransporterName { get; set; }

    public int? Sent { get; set; }

    public int CmpId { get; set; }

    public string LocId { get; set; }

    public int Idd { get; set; }
}
