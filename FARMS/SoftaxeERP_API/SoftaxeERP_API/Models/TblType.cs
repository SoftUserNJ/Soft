using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblType
{
    public int? Typeid { get; set; }

    public string Vchtype { get; set; }

    public string VchDesc { get; set; }

    public bool? IsMonthly { get; set; }

    public bool? IsActual { get; set; }

    public bool? Eoaa { get; set; }

    public bool? T { get; set; }

    public string Tag { get; set; }

    public bool? VerifySign { get; set; }

    public bool? AppSign { get; set; }

    public bool? PrintStop { get; set; }

    public string Catagory { get; set; }

    public string Catagoryho { get; set; }

    public string AccountCode { get; set; }

    public string VeriFyName { get; set; }

    public bool? SkipVerify { get; set; }

    public string ApprovalName { get; set; }

    public bool? SkipApproval { get; set; }

    public string AuditName { get; set; }

    public bool? SkipAudit { get; set; }

    public string LocId { get; set; }

    public int? CmpId { get; set; }
}
