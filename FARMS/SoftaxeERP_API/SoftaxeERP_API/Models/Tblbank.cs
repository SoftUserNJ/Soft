using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblbank
{
    public int Id { get; set; }

    public string Bank { get; set; }

    public int CompId { get; set; }

    public string LocId { get; set; }

    public string BranchCode { get; set; }

    public string Address { get; set; }

    public string AccNo { get; set; }
}
