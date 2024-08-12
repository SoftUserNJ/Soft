using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string UserName { get; set; }

    public int? CmpId { get; set; }

    public string CmpShortName { get; set; }

    public string LocId { get; set; }

    public string LocaionName { get; set; }

    public string Password { get; set; }

    public bool? Admin { get; set; }

    public string Image { get; set; }

    public string Designation { get; set; }

    public string Cnic { get; set; }

    public string Mobile { get; set; }

    public string Type { get; set; }

    public string Email { get; set; }

    public string Emailpass { get; set; }

    public string Country { get; set; }

    public int? OnlineStatus { get; set; }

    public string ComputerName { get; set; }

    public int? Otid { get; set; }

    public string Farmid { get; set; }

    public string Flockid { get; set; }

    public string Permission { get; set; }

    public string AccessToken { get; set; }

    public DateTime? RegDate { get; set; }

    public string Level { get; set; }

    public bool? Allowedit { get; set; }

    public bool? Allowdelete { get; set; }

    public bool? Allowadd { get; set; }

    public bool? AllowExport { get; set; }

    public int? Sent { get; set; }

    public string Location { get; set; }

    public bool? Dashboard { get; set; }

    public string Status { get; set; }

    public string SignalRid { get; set; }

    public decimal? Recovery { get; set; }

    public decimal? Commission { get; set; }

    public decimal? AboveCommission { get; set; }

    public decimal? Target { get; set; }

    public int? SpId { get; set; }

    public bool? IsSuperAdmin { get; set; }
}
