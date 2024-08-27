using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.APP.Commands.User.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ApplicationResult>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
