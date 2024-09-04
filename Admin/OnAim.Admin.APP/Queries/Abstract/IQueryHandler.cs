using MediatR;

namespace OnAim.Admin.APP.Queries.Abstract
{
    public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
    {
    }
}
