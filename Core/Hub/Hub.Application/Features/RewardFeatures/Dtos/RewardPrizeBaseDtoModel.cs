#nullable disable

using Hub.Domain.Entities;

namespace Hub.Application.Features.RewardFeatures.Dtos;

public class RewardPrizeBaseDtoModel
{
    public int Id{ get; set; }
    public int Value { get; set; }
    public string Type { get; set; }

    public static RewardPrizeBaseDtoModel MapFrom(RewardPrize prize)
    {
        return new RewardPrizeBaseDtoModel
        {
            Id = prize.Id,
            Value = prize.Value,
            Type = prize.PrizeTypeId,
        };
    }
}