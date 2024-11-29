#nullable disable

using Hub.Domain.Entities;
using Hub.Domain.Entities.PromotionCoins;
using Hub.Domain.Enum;
using Newtonsoft.Json;

namespace Hub.Application.Models.PromotionCoin;

public class BaseCreatePromotionCoinModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int PromotionId { get; set; }
    public CoinType CoinType { get; set; }

    public static Domain.Entities.PromotionCoins.PromotionCoin ConvertToEntity(BaseCreatePromotionCoinModel model, int promotionId)
    {
        var coinId = $"{promotionId}_{model.Name}";

        return model switch
        {
            CreatePromotionIncomingCoinModel incoming => new PromotionIncomingCoin(
                coinId,
                incoming.Name,
                incoming.Description,
                incoming.ImageUrl,
                incoming.PromotionId),

            CreatePromotionInternalCoinModel internalCoin => new PromotionInternalCoin(
                coinId,
                internalCoin.Name,
                internalCoin.Description,
                internalCoin.ImageUrl,
                internalCoin.PromotionId),

            CreatePromotionPrizeCoinModel prize => new PromotionPrizeCoin(
                coinId,
                prize.Name,
                prize.Description,
                prize.ImageUrl,
                prize.PromotionId),

            CreatePromotionOutgoingCoinModel outgoing => new PromotionOutgoingCoin(
                coinId,
                outgoing.Name,
                outgoing.Description,
                outgoing.ImageUrl,
                outgoing.PromotionId,
                outgoing.WithdrawOptions.Select(w => new WithdrawOption()).ToList()),

            _ => throw new ArgumentException("Invalid coin type", nameof(model))
        };
    }
}