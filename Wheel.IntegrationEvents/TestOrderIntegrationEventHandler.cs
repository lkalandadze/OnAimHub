using Hub.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Wheel.Shared.Interfaces;

namespace Wheel.IntegrationEvents;

public class TestOrderIntegrationEventHandler : IIntegrationEventHandler<TestOrderIntegrationEventAdapter>
{
    private readonly ILogger<TestOrderIntegrationEventHandler> _logger;

    public TestOrderIntegrationEventHandler(ILogger<TestOrderIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TestOrderIntegrationEventAdapter> context)
    {
        var integrationEvent = context.Message;
        _logger.LogInformation($"Order received: {integrationEvent.OrderId} for User: {integrationEvent.UserId} at {integrationEvent.OrderDate}");

        // Perform the desired action with the event data.

        return Task.CompletedTask;
    }
}