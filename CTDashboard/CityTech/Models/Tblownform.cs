using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class Tblownform
{
    public int Formid { get; set; }

    public string? Formname { get; set; }

    public string? Formdata { get; set; }

    public string? Remarks { get; set; }

    public bool? Mandatory { get; set; }

    public bool? AutoAttach { get; set; }

    public int? Customerid { get; set; }
}
