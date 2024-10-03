﻿using MediatR;

namespace OnAim.Admin.APP.CQRS.Query;

public interface IQueryHandler<in TQuery, TResponse>
: IRequestHandler<TQuery, TResponse>
where TQuery : IQuery<TResponse>
where TResponse : notnull
{
}