using Leaderboard.Application.Features.LeaderboardResultFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.GetPlayerResults;

public class GetPlayerResultsQueryResponse : Response<PagedResponse<PlayerResultModel>>;