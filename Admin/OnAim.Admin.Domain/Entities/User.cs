using Newtonsoft.Json;
using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnAim.Admin.Domain.Entities;

public class User : BaseEntity
{
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
        int? activationCode,
        DateTime? activationCodeExpiration,
        bool isSuperAdmin = false)
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
        ActivationCode = activationCode;
        ActivationCodeExpiration = activationCodeExpiration;
        CreatedBy = createdBy;
        DateCreated = SystemDate.Now;
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

    public int? ActivationCode { get; set; }
    public DateTime? ActivationCodeExpiration { get; set; }
    public bool IsVerified { get; set; }

    public int? ResetCode { get; set; }
    public DateTime? ResetCodeExpiration { get; set; }

    public ICollection<AccessToken> AccessTokens { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }

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
