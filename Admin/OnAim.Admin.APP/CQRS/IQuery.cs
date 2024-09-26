using MediatR;

namespace OnAim.Admin.APP.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
where TResponse : notnull
{
}
