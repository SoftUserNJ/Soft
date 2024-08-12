using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblChqCancelation
{
    public int? SrNo { get; set; }

    public string ChqBookNo { get; set; }

    public DateTime? ChqDate { get; set; }

    public int? ChqNo { get; set; }

    public string Bcode { get; set; }

    public string BsubCode { get; set; }

    public string Remarks { get; set; }

    public int? FinId { get; set; }

    public string LocId { get; set; }

    public int? Approve { get; set; }

    public int? AppBy { get; set; }

    public int? Sent { get; set; }

    public int Idd { get; set; }
}
