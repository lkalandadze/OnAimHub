#nullable disable

using Hub.Application.Models.Withdraw.WithdrawOption;
using Hub.Domain.Entities.Coins;
using Hub.Domain.Enum;

namespace Hub.Application.Models.Coin;

public abstract class CreateCoinModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }

    public static Domain.Entities.Coins.Coin ConvertToEntity(CreateCoinModel model, int promotionId)
    {
        var coinId = $"{promotionId}_{model.Name}";

        return model switch
        {
            CreateInCoinModel incoming => new InCoin(
                coinId,
                incoming.Name,
                incoming.Description,
                incoming.ImageUrl,
                promotionId),

            CreateInternalCoinModel internalCoin => new InternalCoin(
                coinId,
                internalCoin.Name,
                internalCoin.Description,
                internalCoin.ImageUrl,
                promotionId),

            CreateAssetCoinModel prize => new AssetCoin(
                coinId,
                prize.Name,
                prize.Description,
                prize.ImageUrl,
                promotionId),

            CreateOutCoinModel outgoing => new OutCoin(
                coinId,
                outgoing.Name,
                outgoing.Description,
                outgoing.ImageUrl,
                promotionId,
                withdrawOptions: outgoing.WithdrawOptions.Select(w => CreateWithdrawOptionModel.ConvertToEntity(w)).ToList()),

            _ => throw new ArgumentException("Invalid coin type", nameof(model))
        };
    }
}