using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblLog
{
    public int Id { get; set; }

    public DateTime? Vdate { get; set; }

    public string Vtype { get; set; }

    public int? Vchno { get; set; }

    public DateTime? VhrDate { get; set; }

    public string Remraks { get; set; }

    public decimal? Amount { get; set; }

    public decimal? PurchaseRate { get; set; }

    public decimal? MinRate { get; set; }

    public decimal? MaxRate { get; set; }

    public int? Uid { get; set; }

    public int? CmpId { get; set; }

    public string Locid { get; set; }

    public int? Finid { get; set; }
}
