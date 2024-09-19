using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.APP.Commands.User.ForgotPassword
{
    public class ForgotPasswordCommand : ICommand<ApplicationResult>
    {
        [Required]
        public string Email { get; set; }
    }
}
