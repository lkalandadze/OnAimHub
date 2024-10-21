using MediatR;
using Shared.IntegrationEvents.Interfaces;
using System.Text.Json.Serialization;

namespace Shared.IntegrationEvents.IntegrationEvents.Player;

public class BanPlayerEvent : DomainEvent, IIntegrationEvent
{
    public BanPlayerEvent(
        int playerId,
        DateTimeOffset? expireDate,
        bool isPermanent,
        string description)
        : base(entityId: playerId, entityType: "Player")
    {
        PlayerId = playerId;
        ExpireDate = expireDate;
        IsPermanent = isPermanent;
        Description = description;
    }

    public int PlayerId { get; }
    public DateTimeOffset? ExpireDate { get; }
    public bool IsPermanent { get; }
    public string Description { get; }

    public Guid CorrelationId => throw new NotImplementedException();
}
public class DomainEvent : INotification
{
    public DomainEvent(int entityId, string entityType)
    {
        EntityId = entityId;
        EntityType = entityType;
        EventType = GetType().Name;
        OccurredOn = DateTime.Now;
    }

    [JsonIgnore]
    public int EntityId { get; }

    [JsonIgnore]
    public string EntityType { get; }

    [JsonIgnore]
    public string EventType { get; }

    public DateTime OccurredOn { get; }
}