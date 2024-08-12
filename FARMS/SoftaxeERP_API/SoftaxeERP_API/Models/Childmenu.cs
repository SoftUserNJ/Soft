using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Childmenu
{
    public string Menuid { get; set; }

    public string Name { get; set; }

    public string Id { get; set; }

    public bool? Active { get; set; }
}
