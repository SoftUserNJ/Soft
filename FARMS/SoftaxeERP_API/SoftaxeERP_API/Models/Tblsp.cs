using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblsp
{
    public int Id { get; set; }

    public string Spname { get; set; }

    public int CompId { get; set; }

    public string Locid { get; set; }
}
