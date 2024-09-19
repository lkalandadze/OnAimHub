using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.User;

namespace OnAim.Admin.APP.Commands.User.Update
{
    public sealed record UpdateUserCommand(int Id, UpdateUserRequest Model) : ICommand<ApplicationResult>;
}
