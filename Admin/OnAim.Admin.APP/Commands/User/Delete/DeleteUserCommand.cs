using MediatR;

namespace OnAim.Admin.APP.Commands.User.Delete
{
    public record DeleteUserCommand(int UserId) : IRequest;
}
