using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblRequisitionMain
{
    public string VchType { get; set; }

    public int? VchNo { get; set; }

    public DateTime? VchDatem { get; set; }

    public int? FinId { get; set; }

    public string LocId { get; set; }

    public int? Sent { get; set; }

    public int Idd { get; set; }

    public int CmpId { get; set; }
}
