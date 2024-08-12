using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblBrand
{
    public int Id { get; set; }

    public int CompId { get; set; }

    public string Brand { get; set; }

    public string Locid { get; set; }
}
