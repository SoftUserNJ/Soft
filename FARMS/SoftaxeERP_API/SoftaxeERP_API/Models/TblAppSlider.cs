using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblAppSlider
{
    public int Id { get; set; }

    public int CompId { get; set; }

    public string SliderPath { get; set; }

    public int? Sort { get; set; }
}
