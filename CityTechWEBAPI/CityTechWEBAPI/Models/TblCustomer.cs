using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblCustomer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public string? BusinessName { get; set; }

    public string? VatNo { get; set; }

    public string? ChamberOfCommerceNo { get; set; }
}
