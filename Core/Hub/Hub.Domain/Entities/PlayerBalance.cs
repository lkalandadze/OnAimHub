#nullable disable

using Hub.Domain.Entities.Coins;
using OnAim.Lib.CodeGeneration.GloballyVisibleClassSharing.Attributes;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

[GloballyVisible]
public class PlayerBalance : BaseEntity<int>
{
    public PlayerBalance()
    {
        
    }
    //Player balances need to be reconfigured. added nullable promotionId so it can work
    public PlayerBalance(decimal amount, int playerId, string coinId, int promotionId)
    {
        Amount = amount;
        PlayerId = playerId;
        CoinId = coinId;
        PromotionId = promotionId;
    }

    public decimal Amount { get; private set; }

    public int PlayerId { get; private set; }
    public Player Player { get; private set; }

    public string CoinId { get; private set; }
    public Coin Coin { get; private set; }


    public int PromotionId { get; private set; }
    public Promotion Promotion { get; private set; }

    public void SetAmount(decimal amount)
    {
        Amount = amount;
    }
}