using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblSubParty
{
    public int Id { get; set; }

    public string DmCode { get; set; }

    public string Code { get; set; }

    public string SubParty { get; set; }

    public int? Area { get; set; }

    public string SubPartyUrdu { get; set; }

    public double? FrgPb { get; set; }

    public int? Sent { get; set; }

    public int CmpId { get; set; }
}
