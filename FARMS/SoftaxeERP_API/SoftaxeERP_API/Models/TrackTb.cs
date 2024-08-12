using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TrackTb
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public DateTime? Date { get; set; }

    public string Status { get; set; }

    public double? Lat { get; set; }

    public double? Lan { get; set; }

    public int? Cmpid { get; set; }

    public string Time { get; set; }
}
