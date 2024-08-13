using MediatR;

namespace OnAim.Admin.APP.Commands.Role.Delete
{
    public record DeleteRoleCommand(string Id): IRequest;
}
