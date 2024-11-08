using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Segment;

public class DeleteSegmentEvent : IIntegrationEvent
{
    public DeleteSegmentEvent(Guid correlationId, string segmentId)
    {
        CorrelationId = correlationId;
        SegmentId = segmentId;
    }
    public Guid CorrelationId { get; set; }
    public string SegmentId { get; set; }
}
