using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblsaleman
{
    public int Id { get; set; }

    public string Saleman { get; set; }

    public int? Otid { get; set; }

    public int CompId { get; set; }

    public string Locid { get; set; }
}
