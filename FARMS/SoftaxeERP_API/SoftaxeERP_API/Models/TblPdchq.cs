using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblPdchq
{
    public string VchType { get; set; }

    public int? VchNo { get; set; }

    public DateTime? VchDate { get; set; }

    public DateTime? ChqDate { get; set; }

    public DateTime? RefundDate { get; set; }

    public DateTime? BounceDate { get; set; }

    public string ChqNo { get; set; }

    public string DmCode { get; set; }

    public string Code { get; set; }

    public string Mcode { get; set; }

    public string DesCrp { get; set; }

    public long? Amount { get; set; }

    public int? FinId { get; set; }

    public string LocId { get; set; }

    public int? Uid { get; set; }

    public string AppBy { get; set; }

    public string BouncedBy { get; set; }

    public string ClearedBy { get; set; }

    public bool? Approve { get; set; }

    public bool? Bounce { get; set; }

    public bool? Clear { get; set; }

    public bool? Reverse { get; set; }

    public string ReverseBy { get; set; }

    public int? Bg1 { get; set; }

    public int? Sent { get; set; }

    public string Refundedby { get; set; }

    public bool? Refund { get; set; }

    public DateTime? DepositDate { get; set; }

    public string Tvchtype { get; set; }

    public string Tmcode { get; set; }

    public int? Tvchno { get; set; }

    public int? Update { get; set; }

    public int Idd { get; set; }

    public int? CmpId { get; set; }
}
