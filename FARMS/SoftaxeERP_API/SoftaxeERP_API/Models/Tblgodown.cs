using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblgodown
{
    public int Godownid { get; set; }

    public string Godownname { get; set; }

    public int CompId { get; set; }

    public string Locid { get; set; }

    public string Alias { get; set; }
}
