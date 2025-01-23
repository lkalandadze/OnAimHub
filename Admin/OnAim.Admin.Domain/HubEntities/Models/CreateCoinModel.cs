using MongoDB.Bson;
using OnAim.Admin.Domain.HubEntities.Coin;
using OnAim.Admin.Domain.HubEntities.Enum;

namespace OnAim.Admin.Domain.HubEntities.Models;

public abstract class CreateCoinModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public string? TemplateId { get; set; }

    public static Coin.Coin ConvertToEntity(
        CreateCoinModel model,
        int promotionId,
        IEnumerable<WithdrawOption> withdrawOptions = null,
        IEnumerable<WithdrawOptionGroup> withdrawOptionGroups = null)
    {
        var coinId = ObjectId.GenerateNewId().ToString();

        return model switch
        {
            CreateInCoinModel incoming => new InCoin(
                coinId,
                incoming.Name,
                incoming.Description,
                incoming.ImageUrl,
                promotionId,
                incoming.Configurations,
                incoming.TemplateId
                ),

            CreateInternalCoinModel internalCoin => new InternalCoin(
                coinId,
                internalCoin.Name,
                internalCoin.Description,
                internalCoin.ImageUrl,
                promotionId),

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
                withdrawOptions: withdrawOptions,
                withdrawOptionGroups: withdrawOptionGroups),

            _ => throw new ArgumentException("Invalid coin type", nameof(model))
        };
    }
}