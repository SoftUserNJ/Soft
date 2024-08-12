using System;
using System.Collections.Generic;

namespace MediaOutDoor.Models;

public partial class TblCustomer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Gender { get; set; }

    public DateTime? Dob { get; set; }

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string ContactNo { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? ProfilePic { get; set; }

    public bool? ActiveStatus { get; set; }

    public string? Otp { get; set; }

    public string? Type { get; set; }

    public string? Fcmtoken { get; set; }

    public string? AccessToken { get; set; }
}
