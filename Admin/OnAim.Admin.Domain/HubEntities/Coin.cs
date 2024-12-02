using OnAim.Admin.Domain.HubEntities;

public abstract class Coin : BaseEntity<string>
{
    public Coin()
    {

    }

    public Coin(string id,
         string name,
         string description,
         string imageUrl,
         CoinType coinType,
         int promotionId,
         int? templateId = null)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;
        PromotionId = promotionId;
        FromTemplateId = templateId;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
    public int? FromTemplateId { get; private set; }

    public int PromotionId { get; set; }
    public Promotion Promotion { get; set; }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.Now;
    }
}
public class CreateCoinModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }

    public static Coin ConvertToEntity(CreateCoinModel model, int promotionId)
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
public class CreateInCoinModel : CreateCoinModel
{
    // should be add configuration in future

    public CreateInCoinModel()
    {
        CoinType = CoinType.In;
    }
}
public class CreateInternalCoinModel : CreateCoinModel
{
    // should be add configuration in future

    public CreateInternalCoinModel()
    {
        CoinType = CoinType.Internal;
    }
}
public class CreateAssetCoinModel : CreateCoinModel
{
    // should be add configuration in future

    public CreateAssetCoinModel()
    {
        CoinType = CoinType.Asset;
    }
}
public class CreateOutCoinModel : CreateCoinModel
{
    public IEnumerable<CreateWithdrawOptionModel> WithdrawOptions { get; set; }

    public CreateOutCoinModel()
    {
        CoinType = CoinType.Out;
    }
}
public class CreateWithdrawOptionModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public static WithdrawOption ConvertToEntity(CreateWithdrawOptionModel model)
    {
        return new()
        {
            Title = model.Title,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            Endpoint = model.Endpoint,
            ContentType = model.ContentType,
            EndpointContent = model.EndpointContent,
        };
    }
}