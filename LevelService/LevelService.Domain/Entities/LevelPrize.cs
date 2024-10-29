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

    public int Amount { get; set; }
    public string PrizeTypeId { get; set; }
    public PrizeType PrizeType { get; set; }
    public PrizeDeliveryType PrizeDeliveryType { get; set; }
    public int RankId { get; set; }

    public void Update(int amount, string prizeTypeId, PrizeDeliveryType prizeDeliveryType)
    {
        Amount = amount;
        PrizeTypeId = prizeTypeId;
        PrizeDeliveryType = prizeDeliveryType;
    }
}