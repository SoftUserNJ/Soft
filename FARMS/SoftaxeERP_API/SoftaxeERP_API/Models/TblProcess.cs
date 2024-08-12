using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblProcess
{
    public int Id { get; set; }

    public string Des { get; set; }

    public int CompId { get; set; }

    public string Calculation { get; set; }

    public string PrOf { get; set; }

    public string IssueT { get; set; }

    public string CrCode { get; set; }

    public string Finid { get; set; }

    public int? Locid { get; set; }
}
