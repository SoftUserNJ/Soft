using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Customer
{
    public string ContactName { get; set; }

    public int? Cid { get; set; }

    public string Country { get; set; }

    public int? Cityid { get; set; }

    public string City { get; set; }
}
