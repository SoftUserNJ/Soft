using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Menu
{
    public string Id { get; set; }

    public string Name { get; set; }

    public bool? Active { get; set; }

    public int? CompId { get; set; }
}
