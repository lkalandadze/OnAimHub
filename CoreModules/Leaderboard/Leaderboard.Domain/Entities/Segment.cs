using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class Segment : BaseEntity<string>
{
    public Segment()
    {
        
    }
    public Segment(string id, string description, int priorityLevel, bool isDeleted)
    {
        Id = id;
        Description = description;
        PriorityLevel = priorityLevel;
        IsDeleted = isDeleted;
    }
    public string Description { get; set; }
    public int PriorityLevel { get; set; }
    public bool IsDeleted { get; set; }
}
