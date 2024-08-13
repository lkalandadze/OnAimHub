using MassTransit;

namespace Hub.Shared.Interfaces;

public interface IIntegrationEventHandler<T> : IConsumer<T>
    where T : class, IIntegrationEvent
{

}
