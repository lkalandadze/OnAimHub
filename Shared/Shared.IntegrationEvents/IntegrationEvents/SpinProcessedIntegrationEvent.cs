using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents;

public class SpinProcessedIntegrationEvent : IIntegrationEvent
{
    public Guid CorrelationId { get; private set; }
    public Guid UserId { get; private set; }
    public bool IsWin { get; private set; }
    public decimal Winnings { get; private set; }
    public decimal RemainingBalance { get; private set; }

    public SpinProcessedIntegrationEvent(Guid correlationId, Guid userId, bool isWin, decimal winnings, decimal remainingBalance)
    {
        CorrelationId = correlationId;
        UserId = userId;
        IsWin = isWin;
        Winnings = winnings;
        RemainingBalance = remainingBalance;
    }
}
