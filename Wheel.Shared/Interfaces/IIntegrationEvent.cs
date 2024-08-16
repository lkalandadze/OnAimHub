namespace Wheel.Shared.Interfaces;

public interface IIntegrationEvent
{
    Guid CorrelationId { get; }
}
