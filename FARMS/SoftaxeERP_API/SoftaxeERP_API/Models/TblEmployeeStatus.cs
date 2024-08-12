using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblEmployeeStatus
{
    public int Id { get; set; }

    public string Description { get; set; }

    public int? CompId { get; set; }

    public string LocId { get; set; }
}
