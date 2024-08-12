using System;
using System.Collections.Generic;

namespace MediaOutdoor_Backend.Models;

public partial class TblCpm
{
    public int Id { get; set; }

    public string? BudgetFrom { get; set; }

    public string? BudgetTo { get; set; }

    public string? Discount { get; set; }

    public string? Reach { get; set; }
}
