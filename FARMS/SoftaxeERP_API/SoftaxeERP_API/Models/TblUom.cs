using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblUom
{
    public int Id { get; set; }

    public string Uom { get; set; }

    public int? Divuom { get; set; }

    public int CompId { get; set; }
}
