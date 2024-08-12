using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblChild
{
    public int Id { get; set; }

    public int CmpId { get; set; }

    public int EmpyId { get; set; }

    public string Name { get; set; }

    public string Gender { get; set; }

    public string Cnic { get; set; }

    public int SrNo { get; set; }

    public string LocId { get; set; }
}
