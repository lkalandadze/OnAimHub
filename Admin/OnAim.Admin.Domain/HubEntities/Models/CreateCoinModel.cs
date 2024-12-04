using OnAim.Admin.Domain.HubEntities.Coin;
using OnAim.Admin.Domain.HubEntities.Enum;

namespace OnAim.Admin.Domain.HubEntities.Models;

public class CreateCoinModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public int? TemplateId { get; set; }

    public static Coin.Coin ConvertToEntity(
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

            CreateAssetCoinModel prize => new AssetCoin(
                coinId,
                prize.Name,
                prize.Description,
                prize.ImageUrl,
                promotionId,
                prize.TemplateId),

            CreateOutCoinModel outgoing => new OutCoin(
                coinId,
                outgoing.Name,
                outgoing.Description,
                outgoing.ImageUrl,
                promotionId,
                withdrawOptions: withdrawOptions,
                withdrawOptionGroups: withdrawOptionGroups
                ),

            _ => throw new ArgumentException("Invalid coin type", nameof(model))
        };
    }
}