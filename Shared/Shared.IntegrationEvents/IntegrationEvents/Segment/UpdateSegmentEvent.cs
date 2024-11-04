using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Segment;

public class UpdateSegmentEvent : IIntegrationEvent
{
    public UpdateSegmentEvent(string id, Guid correlationId, string description, int priorityLevel)
    {
        Id = id;
        CorrelationId = correlationId;
        Description = description;
        PriorityLevel = priorityLevel;
    }
    public string Id { get; set; }
    public Guid CorrelationId { get; }
    public string Description { get; }
    public int PriorityLevel { get; }
}
