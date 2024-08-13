using Hub.Shared.Interfaces;

namespace Hub.IntegrationEvents;

public class TestOrderIntegrationEvent : IIntegrationEvent
{
    public Guid CorrelationId { get; }
    public Guid OrderId { get; }
    public Guid UserId { get; }
    public DateTimeOffset OrderDate { get; }

    public TestOrderIntegrationEvent(Guid correlationId, Guid orderId, Guid userId, DateTimeOffset orderDate)
    {
        CorrelationId = correlationId;
        OrderId = orderId;
        UserId = userId;
        OrderDate = orderDate;
    }
}