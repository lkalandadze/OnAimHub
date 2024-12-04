namespace OnAim.Admin.Domain.HubEntities.Models;

public class PromotionOutgoingCoin : Coin.Coin
{
    public ICollection<WithdrawOption> WithdrawOptions { get; private set; }
}
