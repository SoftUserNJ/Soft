using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblot
{
    public int Id { get; set; }

    public int Otid { get; set; }

    public int? Spid { get; set; }

    public string Otname { get; set; }

    public int? Areaiid { get; set; }

    public string Allow { get; set; }

    public int CompId { get; set; }

    public string Filer { get; set; }

    public string Locid { get; set; }
}
