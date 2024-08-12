using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblLogInOut
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public DateTime? Login { get; set; }

    public DateTime? Logout { get; set; }

    public string Location { get; set; }
}
