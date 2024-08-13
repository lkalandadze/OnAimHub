using Hub.IntegrationEvents;
using Wheel.Shared.Interfaces;

namespace Wheel.IntegrationEvents;

public class TestOrderIntegrationEventAdapter : IIntegrationEvent
{
    public TestOrderIntegrationEvent OriginalEvent { get; }

    public TestOrderIntegrationEventAdapter(TestOrderIntegrationEvent originalEvent)
    {
        OriginalEvent = originalEvent;
    }

    public Guid CorrelationId => OriginalEvent.CorrelationId;
    public Guid OrderId => OriginalEvent.OrderId;
    public Guid UserId => OriginalEvent.UserId;
    public DateTimeOffset OrderDate => OriginalEvent.OrderDate;
}