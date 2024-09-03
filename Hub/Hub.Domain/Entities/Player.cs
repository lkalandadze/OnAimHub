using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class Player : BaseEntity<int>
{
    public string UserName { get; set; }
    public List<int> SegmentIds { get; set; }

    public ICollection<PlayerBalance> PlayerBalances { get; set; }
}