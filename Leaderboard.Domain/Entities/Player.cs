using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class Player : BaseEntity<int>
{
    public Player()
    {
        
    }
    public Player(int id, string userName)
    {
        Id = id;
        UserName = userName;
    }
    public string UserName { get; private set; }
}
