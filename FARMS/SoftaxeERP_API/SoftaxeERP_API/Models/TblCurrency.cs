using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblCurrency
{
    public int Id { get; set; }

    public int CmpId { get; set; }

    public string Country { get; set; }

    public string CurrencyName { get; set; }

    public string CurrencyShortName { get; set; }

    public string CurrencySymbol { get; set; }

    public string CurrentRate { get; set; }

    public string CnameDebit { get; set; }

    public string CnameCredit { get; set; }

    public bool? Basecurrency { get; set; }

    public int Idd { get; set; }
}
