using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblChqBook
{
    public int? SrNo { get; set; }

    public string ChqBookNo { get; set; }

    public DateTime? ChqDate { get; set; }

    public long? ChqFrom { get; set; }

    public long? ChqTo { get; set; }

    public string Bcode { get; set; }

    public string BsubCode { get; set; }

    public string Remarks { get; set; }

    public int CmpId { get; set; }

    public int? FinId { get; set; }

    public string LocId { get; set; }

    public int? Approve { get; set; }

    public int? AppBy { get; set; }

    public int? Sent { get; set; }

    public int? Endbook { get; set; }

    public int Idd { get; set; }

    public string ChqNo { get; set; }
}
