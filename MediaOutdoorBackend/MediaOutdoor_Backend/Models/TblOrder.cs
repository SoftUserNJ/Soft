using System;
using System.Collections.Generic;

namespace MediaOutdoor_Backend.Models;

public partial class TblOrder
{
    public int OrderId { get; set; }

    public string? OrderNo { get; set; }

    public DateTime Date { get; set; }

    public int? CustomerId { get; set; }

    public int? StationId { get; set; }

    public int? ScreenId { get; set; }

    public double? Rate { get; set; }

    public double? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string Status { get; set; } = null!;

    public string? OrderImage { get; set; }

    public string? Remarks { get; set; }

    public string? VisitorId { get; set; }

    public string OrderType { get; set; } = null!;

    public string? Text1 { get; set; }

    public string? Text2 { get; set; }

    public string? Text3 { get; set; }

    public string? Text1Color { get; set; }

    public string? Text2Color { get; set; }

    public string? Text3Color { get; set; }

    public string? Text1Font { get; set; }

    public string? Text2Font { get; set; }

    public string? Text3Font { get; set; }

    public string? Text1TopPosition { get; set; }

    public string? Text1LeftPosition { get; set; }

    public string? Text2TopPosition { get; set; }

    public string? Text2LeftPosition { get; set; }

    public string? Text3TopPosition { get; set; }

    public string? Text3LeftPosition { get; set; }

    public string? Text1Size { get; set; }

    public string? Text2Size { get; set; }

    public string? Text3Size { get; set; }
}
