using System;
using System.Collections.Generic;

namespace CityTech.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int UserTypeId { get; set; }

    public int SkillId { get; set; }

    public string? FcmToken { get; set; }

    public string? FirstName { get; set; }

    public string? SecondName { get; set; }

    public string? Gender { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? EmergencyNo { get; set; }

    public string? AccessToken { get; set; }

    public string? Otp { get; set; }

    public bool? AppAccess { get; set; }

    public bool? DashboardAccess { get; set; }

    public bool? ReceiveIncAlert { get; set; }

    public string? ImgPath { get; set; }

    public bool? AllowCall { get; set; }

    public bool? AllowSms { get; set; }

    public bool? AllowWhatsapp { get; set; }

    public bool? AllowEmail { get; set; }

    public DateTime? InTime { get; set; }

    public DateTime? OutTime { get; set; }

    public bool? IsActive { get; set; }
}
