using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Commands.User.AssignRole
{
    public record AssignRoleToUserCommand(string UserId, string RoleId) : IRequest<ApplicationResult>;
}
