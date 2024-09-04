using MediatR;

namespace OnAim.Admin.APP.Queries.Abstract
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
    {
    }
}
