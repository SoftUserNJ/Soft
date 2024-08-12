using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblvchtype
{
    public int Id { get; set; }

    public string Vchtype { get; set; }

    public int? Sno { get; set; }

    public string Tage { get; set; }

    public string Action { get; set; }

    public string Description { get; set; }

    public string VerifyName { get; set; }

    public bool? VerifyRequired { get; set; }

    public string ApprovalName { get; set; }

    public bool? ApprovalRequired { get; set; }

    public string AuditName { get; set; }

    public string Lasttext { get; set; }

    public int Idd { get; set; }

    public int CmpId { get; set; }
}
