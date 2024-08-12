using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblPaymentTax
{
    public int Id { get; set; }

    public int CompId { get; set; }

    public string PtaxName1 { get; set; }

    public string PtaxName1Code { get; set; }

    public string PtaxName2 { get; set; }

    public string PtaxName1Code2 { get; set; }
}
