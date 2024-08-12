using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class TblUom
{
    public int Id { get; set; }

    public string Uom { get; set; } = null!;
}
