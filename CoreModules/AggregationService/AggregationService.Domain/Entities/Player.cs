using Shared.Domain.Entities;

namespace AggregationService.Domain.Entities;

public class Player : BaseEntity<int>
{
    public Player(int playerId, string username)
    {
        PlayerId = playerId;
        Username = username;
    }
    public int PlayerId { get; set; }
    public string Username { get; set; }
}
