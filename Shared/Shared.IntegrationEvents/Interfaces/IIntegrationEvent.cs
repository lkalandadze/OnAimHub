namespace Shared.IntegrationEvents.Interfaces;

public interface IIntegrationEvent
{
    Guid CorrelationId { get; }
}
