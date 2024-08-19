using MediatR;

namespace OnAim.Admin.APP.Commands.Role.Delete
{
    public record DeleteRoleCommand(int Id): IRequest;
}
