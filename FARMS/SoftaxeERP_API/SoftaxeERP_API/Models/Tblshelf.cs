using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblshelf
{
    public int Godownid { get; set; }

    public int Rackno { get; set; }

    public int Shelfno { get; set; }

    public string Shelfname { get; set; }

    public int CompId { get; set; }

    public string Locid { get; set; }

    public string Alias { get; set; }

    public string Sku { get; set; }
}
