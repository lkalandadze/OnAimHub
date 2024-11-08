using Leaderboard.Application.Features.LeaderboardResultFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.Get;

public class GetLeaderboardResultQueryResponse : Response<PagedResponse<LeaderboardResultModel>>;