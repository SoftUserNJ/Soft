using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Tblbooking
{
    public int Bookingno { get; set; }

    public DateTime? Bookingdate { get; set; }

    public string Code { get; set; }

    public string Mcode { get; set; }

    public double? Qty { get; set; }

    public double? Rate { get; set; }

    public double? Amount { get; set; }

    public double? Dlrate { get; set; }

    public double? DlAmount { get; set; }

    public string Remarks { get; set; }

    public int? Finid { get; set; }

    public int? CmpId { get; set; }

    public int Idd { get; set; }

    public string Uid { get; set; }

    public int? Aprove { get; set; }

    public int? Verify { get; set; }

    public string Appby { get; set; }

    public string Verifyby { get; set; }

    public int? Auditby { get; set; }

    public string Auditbyn { get; set; }

    public int? Reject { get; set; }

    public string Rejectedby { get; set; }

    public string Locid { get; set; }

    public string Sno { get; set; }

    public string VchType { get; set; }

    public int? Completed { get; set; }

    public string Uom { get; set; }

    public int? Divuom { get; set; }

    public string Cropyear { get; set; }

    public string BrokerCode { get; set; }

    public double? BrokerComm { get; set; }

    public string BrokerCommUom { get; set; }

    public string DeliveryTerm { get; set; }

    public string PaymentTerm { get; set; }

    public string InvoiceType { get; set; }

    public int? Currencyid { get; set; }

    public double? ExchangeRate { get; set; }

    public int? Sent { get; set; }
}
