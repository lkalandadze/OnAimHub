#nullable disable

using Hub.Domain.Entities.Coins;
using Hub.Domain.Entities.DbEnums;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class RewardPrize : BaseEntity<int>
{
    public RewardPrize()
    {
        
    }

    public RewardPrize(int value, string coinId)
    {
        Value = value;
        CoinId = coinId;
    }

    public int Value { get; private set; }

    public int RewardId { get; private set; }
    public Reward Reward { get; private set; }

    public string CoinId { get; private set; }
    public Coin Coins { get; private set; }
}