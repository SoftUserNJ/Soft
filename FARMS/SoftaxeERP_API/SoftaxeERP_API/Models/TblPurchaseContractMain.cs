using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblPurchaseContractMain
{
    public string VchType { get; set; }

    public int Pono { get; set; }

    public int? Vchmonth { get; set; }

    public int FinId { get; set; }

    public string LocId { get; set; }

    public DateTime Podate { get; set; }

    public string Insurance { get; set; }

    public string Performano { get; set; }

    public DateTime? PerformaDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public DateTime? CoverDate { get; set; }

    public string HsCode { get; set; }

    public DateTime? CoreNoteDate { get; set; }

    public int? Sent { get; set; }

    public int? CmpId { get; set; }
}
