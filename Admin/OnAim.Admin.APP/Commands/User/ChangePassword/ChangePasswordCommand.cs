using MediatR;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.ChangePassword
{
    public record ChangePasswordCommand(string Email, string OldPassword, string NewPassword) : ICommand<ApplicationResult>;
}
