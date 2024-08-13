using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.APP.Models.Request.User
{
    public class LoginUserRequest
    {
        [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
