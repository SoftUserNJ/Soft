using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblsubgroupparty
{
    public int Groupsubid { get; set; }

    public int Groupid { get; set; }

    public string Groupname { get; set; }

    public int? CompId { get; set; }
}
