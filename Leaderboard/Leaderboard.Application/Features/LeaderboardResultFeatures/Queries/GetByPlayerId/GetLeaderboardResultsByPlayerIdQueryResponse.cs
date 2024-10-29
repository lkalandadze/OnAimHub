using Leaderboard.Application.Features.LeaderboardResultFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.GetByPlayerId;

public class GetLeaderboardResultsByPlayerIdQueryResponse : Response<PagedResponse<LeaderboardResultModel>>;