namespace Hub.Shared.Interfaces;

public interface IIntegrationEvent
{
    Guid CorrelationId { get; }
}
