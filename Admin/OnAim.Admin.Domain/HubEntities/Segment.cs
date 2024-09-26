#nullable disable

namespace OnAim.Admin.Domain.HubEntities;

public class Segment : BaseEntity<string>
{
    public Segment()
    {

    }

    public Segment(string id, string description, int priorityLevel, int? createdByUserId = null)
    {
        Id = id;
        Description = description;
        PriorityLevel = priorityLevel;
        CreatedByUserId = createdByUserId;
    }

    public string Description { get; private set; }
    public int PriorityLevel { get; private set; }
    public int? CreatedByUserId { get; private set; }

    public ICollection<PlayerSegment> PlayerSegments { get; set; }

    public void ChangeDetails(string description, int priorityLevel)
    {
        Description = description;
        PriorityLevel = priorityLevel;
    }
}