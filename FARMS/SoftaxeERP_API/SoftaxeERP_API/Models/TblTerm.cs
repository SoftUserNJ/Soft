using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblTerm
{
    public int Id { get; set; }

    public int CompId { get; set; }

    public string Terms { get; set; }
}
