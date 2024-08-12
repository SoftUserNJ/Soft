using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblServiceBill
{
    public int TransNo { get; set; }

    public DateTime TransDate { get; set; }

    public string DmCode { get; set; }

    public string Code { get; set; }

    public string ServiceCode { get; set; }

    public double? BillRate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime BillingDate { get; set; }

    public int TimePeriod { get; set; }

    public int? Installments { get; set; }

    public double? InstallmentsAmount { get; set; }

    public string CustomerName { get; set; }

    public string CustomerContact { get; set; }

    public int? MainAreaId { get; set; }

    public int? SubAreaId { get; set; }

    public double? TotalBill { get; set; }

    public double? Disc { get; set; }

    public double? DiscAmount { get; set; }

    public string Remarks { get; set; }

    public int? TermsId { get; set; }

    public double? NetDue { get; set; }

    public double? AmountPaid { get; set; }

    public double? ReturnAmount { get; set; }

    public string PaymentMethod { get; set; }

    public double? TotalDue { get; set; }

    public int CmpId { get; set; }

    public string LocId { get; set; }

    public int FinId { get; set; }

    public int? VchNo { get; set; }

    public bool? IsSave { get; set; }
}
