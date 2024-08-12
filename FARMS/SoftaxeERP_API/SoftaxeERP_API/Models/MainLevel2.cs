using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class MainLevel2
{
    public string Level1 { get; set; }

    public string Level2 { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }

    public string Names { get; set; }

    public bool? NotChange { get; set; }
}
