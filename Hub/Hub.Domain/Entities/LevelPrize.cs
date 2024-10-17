using Hub.Domain.Entities.DbEnums;
using Hub.Domain.Enum;
using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

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
    public int LevelId { get; set; }

    public void Update(int amount, string prizeTypeId, PrizeDeliveryType prizeDeliveryType)
    {
        Amount = amount;
        PrizeTypeId = prizeTypeId;
        PrizeDeliveryType = prizeDeliveryType;
    }
}
