using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class CompanyGroup
{
    public int GrpId { get; set; }

    public string CompName { get; set; }

    public string CompAdd { get; set; }

    public string City { get; set; }

    public string Ntn { get; set; }

    public string Pologopath { get; set; }

    public string Contact { get; set; }

    public DateTime? PrintDateTime { get; set; }

    public string PrintedBy { get; set; }

    public string HeadingPdt { get; set; }

    public int Idd { get; set; }

    public string Email { get; set; }

    public bool? IsMulti { get; set; }
}
