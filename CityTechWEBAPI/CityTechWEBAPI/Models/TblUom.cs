using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblUom
{
    public int Id { get; set; }

    public string Uom { get; set; } = null!;
}
