using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblArea
{
    public int? Id { get; set; }

    public string Area { get; set; }

    public string Tag { get; set; }

    public int? CompId { get; set; }
}
