using Leaderboard.Application.Features.LeaderboardTemplateFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Queries.Get;

public class GetLeaderboardTemplatesQueryResponse : Response<PagedResponse<LeaderboardTemplatesModel>>;