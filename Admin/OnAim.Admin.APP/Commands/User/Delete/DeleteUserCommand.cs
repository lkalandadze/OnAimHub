using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.Delete
{
    public record DeleteUserCommand(int UserId) : ICommand<ApplicationResult>;
}
