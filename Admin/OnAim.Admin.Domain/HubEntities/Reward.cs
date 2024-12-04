using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.HubEntities.Enum;
using OnAim.Admin.Domain.HubEntities.PlayerEntities;

public class Reward : BaseEntity<int>
{
    public Reward()
    {

    }

    public Reward(int playerId, int sourceId, DateTime expirationDate, IEnumerable<RewardPrize> prizes)
    {
        PlayerId = playerId;
        SourceId = sourceId;
        ExpirationDate = expirationDate;
        Prizes = prizes?.ToList();
        IsClaimed = false;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public bool IsClaimed { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ClaimedAt { get; private set; }
    public DateTime ExpirationDate { get; private set; }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public int SourceId { get; private set; }
    public RewardSource Source { get; private set; }

    public ICollection<RewardPrize> Prizes { get; private set; }

    public void SetAsClaimed()
    {
        IsClaimed = true;
        ClaimedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}