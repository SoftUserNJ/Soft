using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Sale
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public int CompId { get; set; }
}
