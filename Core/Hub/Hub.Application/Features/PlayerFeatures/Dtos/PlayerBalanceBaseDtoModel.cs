#nullable disable

using Hub.Domain.Entities;

namespace Hub.Application.Features.PlayerFeatures.Dtos;

public class PlayerBalanceBaseDtoModel
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Coin { get; set; }
    public int PromotionId { get; set; }

    public static PlayerBalanceBaseDtoModel MapFrom(PlayerBalance balance)
    {
        return new PlayerBalanceBaseDtoModel
        {
            Id = balance.Id,
            Amount = balance.Amount,
            Coin = balance.CoinId.Split('_')[1],
            PromotionId = balance.PromotionId,
        };
    }
}