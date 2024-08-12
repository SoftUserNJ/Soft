using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblallowfrm
{
    public string Menuid { get; set; }

    public int CompId { get; set; }

    public int Userid { get; set; }

    public string Name { get; set; }

    public string Id { get; set; }

    public bool? Active { get; set; }
}
