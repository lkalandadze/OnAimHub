using MassTransit;
using Shared.IntegrationEvents.Interfaces;

namespace Shared.Infrastructure.Bus;

public class MessageBus : IMessageBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBus _bus;

    public MessageBus(IPublishEndpoint publishEndpoint, IBus bus)
    {
        _publishEndpoint = publishEndpoint;
        _bus = bus;
    }

    public async Task Publish<TEvent>(TEvent @event)
        where TEvent : IIntegrationEvent
    {
        await _publishEndpoint.Publish(@event);
    }

    public async Task PublishWithRouting<TEvent>(TEvent @event, string queueName) where TEvent : IIntegrationEvent
    {
        try
        {
            var sendEndpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));
            await sendEndpoint.Send(@event);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to send message to queue {queueName}", ex);
        }
    }
}
