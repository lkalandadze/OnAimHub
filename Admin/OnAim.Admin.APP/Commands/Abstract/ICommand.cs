using MediatR;

namespace OnAim.Admin.APP.Commands.Abstract
{
    public interface ICommand : ICommand<Unit>
    {
    }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
