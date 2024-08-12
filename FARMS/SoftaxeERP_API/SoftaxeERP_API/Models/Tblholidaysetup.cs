using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblholidaysetup
{
    public int Id { get; set; }

    public string Holiday { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }
}
