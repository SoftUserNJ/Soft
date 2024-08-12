using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblAllowCode
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string Code { get; set; }

    public string SubCode { get; set; }

    public int? Sent { get; set; }
}
