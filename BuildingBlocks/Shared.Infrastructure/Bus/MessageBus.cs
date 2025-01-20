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
            Console.WriteLine($"Attempting to send event to queue: {queueName}");
            var sendEndpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));
            await sendEndpoint.Send(@event);
            Console.WriteLine($"Event has been successfully sent to {queueName}. Event data: {System.Text.Json.JsonSerializer.Serialize(@event)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending event to {queueName}. Exception: {ex.Message}");
            throw new InvalidOperationException($"Failed to send message to queue {queueName}", ex);
        }
    }
}
