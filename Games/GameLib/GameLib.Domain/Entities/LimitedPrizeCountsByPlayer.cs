using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class LimitedPrizeCountsByPlayer : BaseEntity<int>
{
    public LimitedPrizeCountsByPlayer()
    {
        
    }

    public LimitedPrizeCountsByPlayer(int playerId, int prizeId)
    {
        PlayerId = playerId;
        PrizeId = prizeId;
    }

    public int PlayerId { get; private set; }
    public int PrizeId { get; private set; }
    public int Count { get; private set; }

    public void IncreaseCount()
    {
        Count++;
    }
}