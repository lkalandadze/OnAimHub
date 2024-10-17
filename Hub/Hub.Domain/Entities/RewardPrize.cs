#nullable disable

using Hub.Domain.Entities.DbEnums;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class RewardPrize : BaseEntity<int>
{
    public RewardPrize()
    {
        
    }

    public RewardPrize(int amount, int currencyId)
    {
        Amount = amount;
        CurrencyId = currencyId;
    }

    public int Amount { get; private set; }

    public int RewardId { get; private set; }
    public Reward Reward { get; private set; }

    public int CurrencyId { get; private set; }
    public Currency Currency { get; private set; }
}