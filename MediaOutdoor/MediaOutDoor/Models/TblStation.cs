﻿using System;
using System.Collections.Generic;

namespace MediaOutDoor.Models;

public partial class TblStation
{
    public int StationId { get; set; }

    public string StationName { get; set; } = null!;

    public string StationImage { get; set; } = null!;

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string Lat { get; set; } = null!;

    public string Long { get; set; } = null!;
}