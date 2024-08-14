using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.IntegrationEvents.IntegrationEvents;
using Shared.IntegrationEvents.Interfaces;
using System.Collections.Concurrent;

namespace Wheel.IntegrationEvents;

public class SpinRequestedIntegrationEventHandler : IIntegrationEventHandler<SpinRequestedIntegrationEvent>
{
    private static readonly ConcurrentDictionary<Guid, decimal> UserBalances = new ConcurrentDictionary<Guid, decimal>();
    private readonly ILogger<SpinRequestedIntegrationEventHandler> _logger;

    public SpinRequestedIntegrationEventHandler(ILogger<SpinRequestedIntegrationEventHandler> logger)
    {
        _logger = logger;

        // Initialize user balance for the example
        var userId = new Guid("d271d93f-f736-4b2d-924d-55fe4b8462d1"); // Example user ID
        UserBalances.TryAdd(userId, 20.00m); // Each user starts with 20 GEL
    }

    public async Task Consume(ConsumeContext<SpinRequestedIntegrationEvent> context)
    {
        var @event = context.Message;
        var userId = @event.UserId;
        var amount = @event.Amount;

        if (!UserBalances.ContainsKey(userId))
        {
            _logger.LogError($"User {userId} not found.");
            return;
        }

        if (UserBalances[userId] < amount)
        {
            _logger.LogWarning($"User {userId} has insufficient balance.");
            return;
        }

        // Deduct the spin cost
        UserBalances[userId] -= amount;

        // Simulate a spin result
        var random = new Random();
        var isWin = random.Next(0, 2) == 1; // 50% chance to win
        var winnings = isWin ? random.Next(10, 101) : 0; // Random win amount between 10 and 100 GEL if win

        if (isWin)
        {
            // Add winnings to the balance
            UserBalances[userId] += winnings;
        }

        var remainingBalance = UserBalances[userId];

        // Publish the spin result
        var spinProcessedEvent = new SpinProcessedIntegrationEvent(@event.CorrelationId, userId, isWin, winnings, remainingBalance);

        await context.Publish(spinProcessedEvent);
    }
}