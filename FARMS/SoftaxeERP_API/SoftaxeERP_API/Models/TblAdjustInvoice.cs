using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblAdjustInvoice
{
    public int Id { get; set; }

    public int CompId { get; set; }

    public int FinId { get; set; }

    public string Vchtype { get; set; }

    public int? Vchno { get; set; }

    public DateTime? VchDate { get; set; }

    public int? InvNo { get; set; }

    public string InvType { get; set; }

    public decimal? RecAmount { get; set; }
}
