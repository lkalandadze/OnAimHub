using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.APP.Models.Request.User
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
