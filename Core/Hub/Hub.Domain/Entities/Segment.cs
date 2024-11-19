#nullable disable

using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class Segment : BaseEntity<string>
{
    public Segment()
    {

    }

    public Segment(string id, string description, int priorityLevel, int? createdByUserId = null)
    {
        Id = id.ToLower();
        Description = description;
        PriorityLevel = priorityLevel;
        CreatedByUserId = createdByUserId;
    }

    public string Description { get; private set; }
    public int PriorityLevel { get; private set; }
    public int? CreatedByUserId { get; private set; }
    public bool IsDeleted { get; set; }

    public ICollection<Player> Players { get; set; }
    public ICollection<Player> BlockedPlayers { get; private set; }
    public ICollection<Promotion> Promotions { get; private set; }

    public void ChangeDetails(string description, int priorityLevel)
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