using LevelService.Domain.Entities.DbEnums;
using LevelService.Domain.Enum;
using Shared.Domain.Entities;

namespace LevelService.Domain.Entities;

public class LevelPrize : BaseEntity<int>
{
    public LevelPrize(int amount, string prizeTypeId, PrizeDeliveryType prizeDeliveryType)
    {
        Amount = amount;
        PrizeTypeId = prizeTypeId;
        PrizeDeliveryType = prizeDeliveryType;
    }

    public int Amount { get; private set; }
    public string PrizeTypeId { get; private set; }
    public PrizeType PrizeType { get; private set; }
    public PrizeDeliveryType PrizeDeliveryType { get; private set; }
    public int RankId { get; private set; }

    public void Update(int amount, string prizeTypeId, PrizeDeliveryType prizeDeliveryType)
    {
        Amount = amount;
        PrizeTypeId = prizeTypeId;
        PrizeDeliveryType = prizeDeliveryType;
    }
}