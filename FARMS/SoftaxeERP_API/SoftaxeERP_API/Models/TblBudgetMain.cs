using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblBudgetMain
{
    public string VchType { get; set; }

    public int? VchNo { get; set; }

    public DateTime? VchDate { get; set; }

    public int? FinId { get; set; }

    public string LocId { get; set; }

    public int? Sent { get; set; }

    public int Idd { get; set; }

    public int? CmpId { get; set; }
}
