using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class TblArticle
{
    public int ArticleNo { get; set; }

    public string Name { get; set; } = null!;

    public int? GroupId { get; set; }

    public string Uom { get; set; } = null!;

    public string? ImgPath { get; set; }
}
