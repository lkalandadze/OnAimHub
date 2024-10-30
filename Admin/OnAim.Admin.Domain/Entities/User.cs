using Newtonsoft.Json;
using OnAim.Admin.Contracts.Dtos.User;
using OnAim.Admin.Contracts.Enums;
using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Contracts.Dtos.User;
using OnAim.Admin.Contracts.Enums;
using OnAim.Admin.Contracts.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnAim.Admin.Domain.Entities;

public class User : BaseEntity
{
    public User()
    {
        
    }
    public User(
        string firstName,
        string lastName,
        string username,
        string email,
        string password,
        string salt,
        string phone,
        int? createdBy,
        bool isVerified,
        bool isActive,
        string? verificationCode,
        VerificationPurpose? verificationPurpose,
        DateTime? verificationCodeExpiration,
        bool isSuperAdmin = false
        )
    {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Email = email;
        Phone = phone;
        Password = password;
        Salt = salt;
        IsSuperAdmin = isSuperAdmin;
        IsDeleted = false;
        IsVerified = isVerified;
        IsActive = isActive;
        CreatedBy = createdBy;
        DateCreated = SystemDate.Now;
        VerificationCode = verificationCode;
        VerificationPurpose = verificationPurpose;
        VerificationCodeExpiration = verificationCodeExpiration;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Email { get; set; }
    public string Salt { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public bool IsDeleted { get; set; }
    public int? CreatedBy { get; set; }
    public bool IsSuperAdmin { get; set; }
    public DateTimeOffset LastLogin { get; set; }

    public bool IsVerified { get; set; }

    public bool? IsTwoFactorEnabled { get; set; }

    public ICollection<AccessToken> AccessTokens { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }

    public string? VerificationCode { get; set; }
    public VerificationPurpose? VerificationPurpose { get; set; }
    public DateTime? VerificationCodeExpiration { get; set; }

    public string PreferencesJson { get; set; } = "{}";
    [NotMapped]
    public UserPreferences Preferences
    {
        get => string.IsNullOrEmpty(PreferencesJson)
            ? new UserPreferences()
            : JsonConvert.DeserializeObject<UserPreferences>(PreferencesJson);
        set => PreferencesJson = JsonConvert.SerializeObject(value ?? new UserPreferences());
    }
}
