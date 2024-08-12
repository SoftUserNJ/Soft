using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblGroup
{
    public int Groupid { get; set; }

    public string Groupname { get; set; }

    public int CompId { get; set; }

    public string Tag { get; set; }

    public string Concode { get; set; }

    public int? Otid { get; set; }

    public string Img { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Rate { get; set; }

    public DateTime? DiscDate { get; set; }

    public int? ExpiryDays { get; set; }

    public bool? IsCommission { get; set; }
}
