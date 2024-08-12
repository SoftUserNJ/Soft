using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblBudgetDetail
{
    public string VchType { get; set; }

    public int? VchNo { get; set; }

    public DateTime? VchDate { get; set; }

    public string DmCode { get; set; }

    public string Code { get; set; }

    public string Descrp { get; set; }

    public int? Qty { get; set; }

    public int? Amount { get; set; }

    public int? FinId { get; set; }

    public string LocId { get; set; }

    public int? Uid { get; set; }

    public string Ym { get; set; }

    public bool? Approve { get; set; }

    public int? AppBy { get; set; }

    public int? SrNo { get; set; }

    public int? Sent { get; set; }

    public int Idd { get; set; }

    public int? CmpId { get; set; }
}
