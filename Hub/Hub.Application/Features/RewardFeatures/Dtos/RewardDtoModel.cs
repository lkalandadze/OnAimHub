#nullable disable

using Hub.Domain.Entities;

namespace Hub.Application.Features.RewardFeatures.Dtos;

public class RewardDtoModel : RewardBaseDtoModel
{
    //public PlayerBaseDtoModel Player { get; set; }
    public IEnumerable<RewardPrizeBaseDtoModel> Prizes { get; set; }

    public static RewardDtoModel MapFrom(Reward reward, bool includeNavProperties = true)
    {
        var model = new RewardDtoModel
        {
            Id = reward.Id,
            IsClaimed = reward.IsClaimed,
            CreatedAt = reward.CreatedAt,
            ClaimedAt = reward.ClaimedAt,
            RewardSource = reward.Source.Name,
        };

        if (includeNavProperties)
        {
            //model.Player = reward.Player != null ? PlayerBaseDtoModel.MapFrom(reward.Player) : null;
            model.Prizes = reward.Prizes != null && reward.Prizes.Any() ? reward.Prizes.Select(p => RewardPrizeBaseDtoModel.MapFrom(p)) : null;
        }

        return model;
    }
}