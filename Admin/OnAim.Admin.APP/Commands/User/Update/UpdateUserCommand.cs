using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.User;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.Update
{
    public sealed record UpdateUserCommand(int Id, UpdateUserRequest Model) : ICommand<ApplicationResult>;
}
