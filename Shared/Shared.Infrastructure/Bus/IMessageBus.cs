using Shared.IntegrationEvents.Interfaces;

namespace Shared.Infrastructure.Bus;

public interface IMessageBus
{
    Task Publish<TEvent>(TEvent @event)
        where TEvent : IIntegrationEvent;
}
