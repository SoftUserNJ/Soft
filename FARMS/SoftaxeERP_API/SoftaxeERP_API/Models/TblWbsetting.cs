using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblWbsetting
{
    public string Baudrate { get; set; }

    public string DataBits { get; set; }

    public string Parity { get; set; }

    public string StopBits { get; set; }

    public string PortName { get; set; }

    public string Baudrate2 { get; set; }

    public string DataBits2 { get; set; }

    public string Parity2 { get; set; }

    public string StopBits2 { get; set; }

    public string PortName2 { get; set; }

    public string SelectedWb { get; set; }

    public int? InLimit { get; set; }

    public int? OutLimit { get; set; }

    public int Idd { get; set; }

    public int? InLimitWeight { get; set; }

    public int? OutLimitWeight { get; set; }

    public int Cmpid { get; set; }

    public string Locid { get; set; }
}
