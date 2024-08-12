using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblrack
{
    public int Godownid { get; set; }

    public int Rackno { get; set; }

    public string Rackname { get; set; }

    public int CompId { get; set; }

    public string Locid { get; set; }

    public string Alias { get; set; }
}
