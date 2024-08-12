using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tbltran
{
    public long? Id { get; set; }

    public string Vchdate { get; set; }

    public string Code { get; set; }

    public int? GodownId { get; set; }

    public int? RackId { get; set; }

    public int? ShelfId { get; set; }

    public double? Balance { get; set; }
}
