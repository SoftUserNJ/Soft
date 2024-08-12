using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblCowHistory
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string Event { get; set; }

    public DateTime? Date { get; set; }

    public string Remarks { get; set; }

    public int? CompId { get; set; }

    public int? Finid { get; set; }

    public string Locid { get; set; }
}
