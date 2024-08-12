using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblVehicleLoan
{
    public int CompId { get; set; }

    public int EmpyId { get; set; }

    public string LocId { get; set; }

    public int Srno { get; set; }

    public DateTime? Stdate { get; set; }

    public int? Noofmnth { get; set; }

    public double? Loanamt { get; set; }

    public double? Instamt { get; set; }

    public string Remarks { get; set; }

    public string Userid { get; set; }

    public string Editby { get; set; }

    public int? Id { get; set; }

    public bool? Active { get; set; }

    public string Vch { get; set; }

    public bool? Sent { get; set; }

    public string Vehicleno { get; set; }

    public double? Opening { get; set; }

    public string Engineno { get; set; }

    public string Chasisno { get; set; }

    public int? Erpentry { get; set; }

    public int Finid { get; set; }

    public string AccountCode { get; set; }

    public bool? FinEntry { get; set; }
}
