using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblArticleGroup
{
    public int Id { get; set; }

    public string GroupDescription { get; set; } = null!;
}
