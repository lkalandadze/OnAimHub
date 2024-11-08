using LevelService.Domain.Enum;

namespace LevelService.Application.Models.LevelPrizes;

public class LevelPrizesModel
{
    public int Amount { get; set; }
    public string PrizeTypeId { get; set; }
    public PrizeDeliveryType PrizeDeliveryType { get; set; }
}
