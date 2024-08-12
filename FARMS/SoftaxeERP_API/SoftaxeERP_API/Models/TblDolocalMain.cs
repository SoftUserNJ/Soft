using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblDolocalMain
{
    public int CmpId { get; set; }

    public string LocId { get; set; }

    public int FinId { get; set; }

    public string VchType { get; set; }

    public int Dono { get; set; }

    public int Vchmonth { get; set; }

    public DateTime Dodate { get; set; }

    public DateTime? DueDate { get; set; }

    public int? DueDays { get; set; }

    public string Cdisc1 { get; set; }

    public int? Disc1 { get; set; }

    public string Cdisc2 { get; set; }

    public int? Disc2 { get; set; }

    public string Cdisc3 { get; set; }

    public int? Disc3 { get; set; }

    public string Cdisc4 { get; set; }

    public int? Disc4 { get; set; }

    public string Cdisc5 { get; set; }

    public int? Disc5 { get; set; }

    public string Cdisc6 { get; set; }

    public int? Disc6 { get; set; }

    public string Cdisc7 { get; set; }

    public int? Disc7 { get; set; }

    public int? Sent { get; set; }

    public int? SendNo { get; set; }

    public double? ToalAmount { get; set; }

    public string Pcode { get; set; }

    public string PsubCode { get; set; }

    public double? Lat { get; set; }

    public double? Lan { get; set; }

    public int? Uid { get; set; }

    public string DoDatetime { get; set; }

    public int? InvNo { get; set; }

    public DateTime? CurrentDate { get; set; }

    public double? ReceiveAmount { get; set; }

    public bool? Delivered { get; set; }
}
