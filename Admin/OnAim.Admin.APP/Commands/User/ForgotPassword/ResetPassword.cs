using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.APP.Commands.User.ForgotPassword
{
    public class ResetPassword : ICommand<ApplicationResult>
    {
        public string Email { get; set; }
        [Required]
        public int Code { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
