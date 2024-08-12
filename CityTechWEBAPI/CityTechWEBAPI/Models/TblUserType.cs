using System;
using System.Collections.Generic;

namespace CityTechWEBAPI.Models;

public partial class TblUserType
{
    public int UserTypeId { get; set; }

    public string UserType { get; set; } = null!;
}
