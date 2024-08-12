using System;
using System.Collections.Generic;

namespace MediaOutdoor_Backend.Models;

public partial class TblContentDetail
{
    public int ContentId { get; set; }

    public int CatId { get; set; }

    public string SingleImage { get; set; } = null!;

    public string DoubleImage { get; set; } = null!;

    public string TrippleImage { get; set; } = null!;
}
