using OnAim.Admin.Shared.DTOs.Endpoint;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.Shared.DTOs.User
{
    public class LoginUserRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
