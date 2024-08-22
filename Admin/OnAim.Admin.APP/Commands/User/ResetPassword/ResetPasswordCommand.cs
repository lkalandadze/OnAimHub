using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.ResetPassword
{
    public sealed record ResetPasswordCommand(int Id, string Password) : IRequest<ApplicationResult>;
}
