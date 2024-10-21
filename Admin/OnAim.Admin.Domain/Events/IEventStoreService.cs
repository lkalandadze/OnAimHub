using Shared.IntegrationEvents.IntegrationEvents.Player;

namespace OnAim.Admin.Domain.Events;

public interface IEventStoreService
{
    void Save<TDomainEvent>(TDomainEvent @event)
        where TDomainEvent : DomainEvent;

    Task SaveAsync<TDomainEvent>(TDomainEvent @event)
        where TDomainEvent : DomainEvent;

    Task<List<TStoredEvent>> GetListAsync<TStoredEvent>(string entityId, string entityType)
        where TStoredEvent : StoredEvent;
}
