using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblLabTestType
{
    public string VchType { get; set; }

    public int? LabTestNo { get; set; }

    public string LabTestName { get; set; }

    public int? Sent { get; set; }

    public int Idd { get; set; }

    public string TestUom { get; set; }

    public int? CompId { get; set; }

    public string LocId { get; set; }
}
