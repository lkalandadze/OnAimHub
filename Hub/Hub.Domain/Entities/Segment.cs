#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Segment : DbEnum<string>
{
    public Segment()
    {
        
    }

    public Segment(string id, string description, int priorityLevel, int? createdByUserId = null, ICollection<PlayerSegment> playerSegments = null)
    {
        Id = id;
        Description = description;
        PriorityLevel = priorityLevel;
        CreatedByUserId = createdByUserId;
        PlayerSegments = playerSegments;
    }

    public static Segment Segment1 => FromId("Segment1");
    public static Segment Segment2 => FromId("Segment2");
    public static Segment Segment3 => FromId("Segment3");

    public string Description { get; private set; }
    public int PriorityLevel { get; private set; }
    public int? CreatedByUserId { get; private set; }

    public ICollection<PlayerSegment> PlayerSegments { get; set; }

    private static Segment FromId(string id)
    {
        return new Segment { Id = id };
    }
}