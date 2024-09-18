#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Player : BaseEntity<int>
{
    public Player()
    {

    }

    public Player(int id, string userName, ICollection<PlayerSegment> playerSegments = null, ICollection<PlayerBalance> playerBalances = null)
    {
        Id = id;
        UserName = userName;
        PlayerSegments = playerSegments;
        PlayerBalances = playerBalances;
    }

    public string UserName { get; private set; }

    public ICollection<PlayerBalance> PlayerBalances { get; private set; }
    public ICollection<PlayerSegment> PlayerSegments { get; private set; } 

    public void ChangeDetails(string userName)
    {
        UserName = userName;
    }

    public void AddPlayerBalances(ICollection<PlayerSegment> playerSegments)
    {
    }
}