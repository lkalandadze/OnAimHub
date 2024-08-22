using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Role.Delete
{
    public record DeleteRoleCommand(int Id): IRequest<ApplicationResult>;
}
