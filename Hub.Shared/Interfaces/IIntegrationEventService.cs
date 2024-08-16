namespace Hub.Shared.Interfaces;

public interface IIntegrationEventService
{
    Task AddAsync(IIntegrationEvent @event);
    Task PublishAllAsync();
    Task ClearAllAsync();
}
