using System;
using System.Collections.Generic;

namespace MediaOutdoor_Backend.Models;

public partial class TblContentCat
{
    public int CatId { get; set; }

    public string Category { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Icon { get; set; }
}
