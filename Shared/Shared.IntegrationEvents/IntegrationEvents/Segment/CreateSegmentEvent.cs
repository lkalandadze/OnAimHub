using Shared.IntegrationEvents.Interfaces;

namespace Shared.IntegrationEvents.IntegrationEvents.Segment;

public class CreateSegmentEvent : IIntegrationEvent
{
    public CreateSegmentEvent(
        string id,
        Guid correlationId, 
        string description, 
        int priorityLevel, 
        bool isDeleted
        )
    {
        Id = id;
        CorrelationId = correlationId;
        Description = description;
        PriorityLevel = priorityLevel;
        IsDeleted = isDeleted;
    }
    public string Id { get; set; }
    public Guid CorrelationId { get; }
    public string Description { get; }
    public int PriorityLevel { get; }
    public bool IsDeleted { get; }
}

