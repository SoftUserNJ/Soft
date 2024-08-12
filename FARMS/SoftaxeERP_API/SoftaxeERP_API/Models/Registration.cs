using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class Registration
{
    public DateTime? RegistrationDate { get; set; }

    public int? RegistredUsers { get; set; }

    public int Id { get; set; }
}
