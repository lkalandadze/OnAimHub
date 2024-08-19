using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.RemoveRole
{
    public record RemoveRoleCommand(int UserId, int RoleId) : IRequest<ApplicationResult>;
}
