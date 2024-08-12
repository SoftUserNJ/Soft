using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblsubgroup
{
    public int Groupid { get; set; }

    public int Groupsubid { get; set; }

    public string Groupname { get; set; }

    public int CompId { get; set; }

    public string Status { get; set; }

    public string Img { get; set; }

    public int? Otid { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Rate { get; set; }

    public DateTime? DiscDate { get; set; }
}
