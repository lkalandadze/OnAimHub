#nullable disable

using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;

namespace Hub.Application.Features.RewardFeatures.Dtos;

public class RewardBaseDtoModel
{
    public int Id { get; set; }
    public bool IsClaimed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ClaimedAt { get; set; }
    public RewardSource RewardSource { get; set; }

    public static RewardBaseDtoModel MapFrom(Reward reward)
    {
        return RewardDtoModel.MapFrom(reward, false);
    }
}