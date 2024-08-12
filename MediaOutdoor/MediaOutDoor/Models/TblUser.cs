using System;
using System.Collections.Generic;

namespace MediaOutDoor.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Gender { get; set; }

    public string? UserType { get; set; }

    public string? UserAddress1 { get; set; }

    public string? UserAddress2 { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? ContactNo { get; set; }

    public string? Email { get; set; }

    public string? ProfilePic { get; set; }

    public bool? ActiveStatus { get; set; }
}
