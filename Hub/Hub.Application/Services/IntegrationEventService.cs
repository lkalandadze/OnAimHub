using Hub.Shared.Interfaces;
using MassTransit;

namespace Hub.Application.Services;

public class IntegrationEventService : IIntegrationEventService
{
    private readonly IBus _eventBus;
    private readonly List<IIntegrationEvent> _events = new List<IIntegrationEvent>();

    public IntegrationEventService(IBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Task AddAsync(IIntegrationEvent @event)
    {
        _events.Add(@event);
        return Task.CompletedTask;
    }

    public async Task PublishAllAsync()
    {
        await Task.WhenAll(_events.Select(e => PublishWithRetry(e, 2)));
        _events.Clear();
    }

    public Task ClearAllAsync()
    {
        _events.Clear();
        return Task.CompletedTask;
    }

    private async Task PublishWithRetry(IIntegrationEvent @event, int retriesLeft)
    {
        try
        {
            await _eventBus.Publish(@event, @event.GetType());
        }
        catch
        {
            if (retriesLeft > 0)
            {
                await PublishWithRetry(@event, --retriesLeft);
            }
            else
            {
                throw;
            }
        }
    }
}
