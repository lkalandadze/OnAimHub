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

    public void Update(string description, int priorityLevel)
    {
        Description = description;
        PriorityLevel = priorityLevel;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void AddPlayers(List<Player> players)
    {
        foreach (var player in players)
        {
            if (!players.Contains(player))
            {
                Players.Add(player);
            }
        }
    }

    public void RemovePlayers(List<Player> players)
    {
        foreach (var player in players)
        {
            if (!players.Contains(player))
            {
                Players.Remove(player);
            }
        }
    }

    public void BlockPlayers(List<Player> players)
    {
        foreach (var player in players)
        {
            if (!players.Contains(player))
            {
                BlockedPlayers.Add(player);
            }
        }
    }

    public void UnblockPlayers(List<Player> players)
    {
        foreach (var player in players)
        {
            if (!players.Contains(player))
            {
                BlockedPlayers.Remove(player);
            }
        }
    }
}
