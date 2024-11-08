using MassTransit;

namespace Shared.IntegrationEvents.Interfaces;

public interface IIntegrationEventHandler<T> : IConsumer<T>
    where T : class, IIntegrationEvent
{
}