#nullable disable

using Hub.Domain.Entities;
using Hub.Domain.Entities.Coins;
using Hub.Domain.Enum;

namespace Hub.Application.Models.Coin;

public abstract class CreateCoinModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }

    public static Domain.Entities.Coins.Coin ConvertToEntity(
        CreateCoinModel model,
        int promotionId,
        IEnumerable<WithdrawOption> withdrawOptions = null,
        IEnumerable<WithdrawOptionGroup> withdrawOptionGroups = null)
    {
        var coinId = $"{promotionId}_{model.Name}";

        return model switch
        {
            CreateInCoinModel incoming => new InCoin(
                coinId,
                incoming.Name,
                incoming.Description,
                incoming.ImageUrl,
                promotionId,
                incoming.TemplateId),

            CreateInternalCoinModel internalCoin => new InternalCoin(
                coinId,
                internalCoin.Name,
                internalCoin.Description,
                internalCoin.ImageUrl,
                promotionId,
                internalCoin.TemplateId),

            CreateAssetCoinModel asset => new AssetCoin(
                coinId,
                asset.Name,
                asset.Description,
                asset.ImageUrl,
                promotionId,
                asset.Value,
                asset.TemplateId),

            CreateOutCoinModel outgoing => new OutCoin(
                coinId,
                outgoing.Name,
                outgoing.Description,
                outgoing.ImageUrl,
                promotionId,
                outgoing.Value,
                outgoing.TemplateId,
                withdrawOptions: withdrawOptions,
                withdrawOptionGroups: withdrawOptionGroups),

            _ => throw new ArgumentException("Invalid coin type", nameof(model))
        };
    }
}