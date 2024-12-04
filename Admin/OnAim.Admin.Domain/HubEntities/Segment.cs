using OnAim.Admin.Domain.HubEntities.PlayerEntities;

namespace OnAim.Admin.Domain.HubEntities;

public class Segment : BaseEntity<string>
{
    public Segment()
    {

    }

    public Segment(string id,
        string description,
        int priorityLevel,
        int? createdByUserId = null,
        IEnumerable<Promotion> promotions = null)
    {
        Id = id.ToLower();
        Description = description;
        PriorityLevel = priorityLevel;
        CreatedByUserId = createdByUserId;
        Promotions = promotions?.ToList() ?? [];
    }

    public string Description { get; private set; }
    public int PriorityLevel { get; private set; }
    public int? CreatedByUserId { get; private set; }
    public bool IsDeleted { get; private set; }

    public ICollection<Player> Players { get; private set; }
    public ICollection<Player> BlockedPlayers { get; private set; }
    public ICollection<Promotion> Promotions { get; private set; }
}
