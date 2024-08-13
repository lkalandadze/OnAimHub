using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.APP.Models.Request.User
{
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
