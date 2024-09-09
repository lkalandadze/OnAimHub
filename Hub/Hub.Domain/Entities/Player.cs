#nullable disable

using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Player : BaseEntity<int>
{
    public Player()
    {

    }

    public Player(int id, string userName, List<int>? segmentIds = null, ICollection<PlayerBalance>? playerBalances = null)
    {
        Id = id;
        UserName = userName;
        SegmentIds = segmentIds;
        PlayerBalances = playerBalances;
    }

    public string UserName { get; private set; }
    public List<int>? SegmentIds { get; private set; }

    public ICollection<PlayerBalance>? PlayerBalances { get; set; }

    public void ChangeDetails(string userName, List<int>? segmentIds = null, ICollection<PlayerBalance>? playerBalances = null)
    {
        UserName = userName;
        SegmentIds = segmentIds;
        PlayerBalances = playerBalances;
    }
}