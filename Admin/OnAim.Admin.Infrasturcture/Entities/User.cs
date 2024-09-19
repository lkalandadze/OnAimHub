using Newtonsoft.Json;
using OnAim.Admin.Infrasturcture.Entities.Abstract;
using OnAim.Admin.Shared.DTOs.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnAim.Admin.Infrasturcture.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public DateTime DateOfBirth { get; set; }
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
}
