using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class TblUsedArticle
{
    public int Id { get; set; }

    public int? IncidentNo { get; set; }

    public int? ArticleId { get; set; }

    public double? Qty { get; set; }

    public string? ArticleImg { get; set; }
}
