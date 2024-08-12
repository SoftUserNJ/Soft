using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class UserDatabase
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Country { get; set; }

    public int? OnlineStatus { get; set; }
}
