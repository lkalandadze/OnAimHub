#nullable disable

using Hub.Domain.Entities.DbEnums;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class RewardPrize : BaseEntity<int>
{
    public RewardPrize()
    {
        
    }

    public RewardPrize(int value)
    {
        Value = value;
    }

    public int Value { get; private set; }

    public int RewardId { get; private set; }
    public Reward Reward { get; private set; }

    public int CurrencyId { get; private set; }
    public Currency Currency { get; private set; }

    public void ChangeDetails(int value, int probability, int prizeTypeId)
    {
        Value = value;
    }
}