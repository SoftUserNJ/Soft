using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblCompnay
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Telephone { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? ImgPath { get; set; }
}
