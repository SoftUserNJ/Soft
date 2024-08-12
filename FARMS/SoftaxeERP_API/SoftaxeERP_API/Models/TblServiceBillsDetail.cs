using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblServiceBillsDetail
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string ProductName { get; set; }

    public double? CostRate { get; set; }

    public double? ProductRate { get; set; }

    public double? Qty { get; set; }

    public double? ProductTax { get; set; }

    public double? PtaxAmount { get; set; }

    public double? Ptotal { get; set; }

    public string Service { get; set; }

    public string ServiceId { get; set; }

    public int? TransNoId { get; set; }

    public string StockCode { get; set; }

    public int CmpId { get; set; }

    public string LocId { get; set; }

    public int FinId { get; set; }

    public string Remarks { get; set; }

    public int? GodownId { get; set; }

    public int? RackId { get; set; }

    public int? ShelfId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int? UomId { get; set; }
}
