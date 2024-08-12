using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tbldashboard
{
    public int Id { get; set; }

    public string Time { get; set; }

    public int? CompId { get; set; }
}
