using Hub.Application.Features.RewardFeatures.Dtos;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.RewardFeatures.Queries.GetPlayerRewards;

public class GetPlayerRewardsQueryResponse : Response<IEnumerable<RewardDtoModel>>
{

}