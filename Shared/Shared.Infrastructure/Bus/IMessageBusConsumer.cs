﻿namespace Shared.Infrastructure.Bus;

public interface IMessageBusConsumer<TConsumer, T>
{
    Task HandleAsync(T data, MetaData metaData, CancellationToken cancellationToken = default);
}
