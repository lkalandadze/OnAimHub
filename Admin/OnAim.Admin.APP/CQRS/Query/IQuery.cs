namespace OnAim.Admin.APP.CQRS.Query;

public interface IQuery<out TResponse> : IRequest<TResponse>
where TResponse : notnull
{
}
