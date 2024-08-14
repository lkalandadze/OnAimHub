using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.IntegrationEvents.IntegrationEvents;
using Shared.IntegrationEvents.Interfaces;

namespace Hub.IntegrationEvents;

public class SpinProcessedIntegrationEventHandler : IIntegrationEventHandler<SpinProcessedIntegrationEvent>
{
    private readonly ILogger<SpinProcessedIntegrationEventHandler> _logger;

    public SpinProcessedIntegrationEventHandler(ILogger<SpinProcessedIntegrationEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<SpinProcessedIntegrationEvent> context)
    {
        var @event = context.Message;
        _logger.LogInformation($"User {@event.UserId} spin result: {@event.IsWin}, Winnings: {@event.Winnings}, Remaining Balance: {@event.RemainingBalance}, CorrelationId: {@event.CorrelationId}");
        // Further processing can be done here, like updating the user dashboard, sending notifications, etc.
        return Task.CompletedTask;
    }
}