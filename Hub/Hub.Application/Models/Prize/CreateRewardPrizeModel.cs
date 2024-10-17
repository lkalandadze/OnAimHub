#nullable disable

namespace Hub.Application.Models.Prize;

public class CreateRewardPrizeModel
{
    public int Amount { get; set; }
    public string PrizeTypeId { get; set; }
}