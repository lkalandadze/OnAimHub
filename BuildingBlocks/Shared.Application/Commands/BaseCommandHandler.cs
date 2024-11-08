using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.Interfaces;

namespace Shared.Application.Commands;

public class BaseCommandHandler
{
    private readonly IMessageBus _messageBus;

    public BaseCommandHandler(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    protected async Task PublishIntegrationEventAsync<TEvent>(TEvent @event)
     where TEvent : IIntegrationEvent
    {
        await _messageBus.Publish(@event);
    }
}