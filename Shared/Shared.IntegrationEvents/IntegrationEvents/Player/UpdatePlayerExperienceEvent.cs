using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Player;

public class UpdatePlayerExperienceEvent : IIntegrationEvent
{
    public UpdatePlayerExperienceEvent(
        Guid correlationId,
        int amount,
        string currencyId,
        int playerId
        )
    {
        CorrelationId = correlationId;
        Amount = amount;
        CurrencyId = currencyId;
        PlayerId = playerId;
    }
    public Guid CorrelationId { get; set; }
    public int Amount { get; set; }
    public string CurrencyId { get; set; }
    public int PlayerId { get; set; }
}