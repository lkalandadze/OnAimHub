using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.AssignRole
{
    public record AssignRoleToUserCommand(int UserId, int RoleId) : IRequest<ApplicationResult>;
}
