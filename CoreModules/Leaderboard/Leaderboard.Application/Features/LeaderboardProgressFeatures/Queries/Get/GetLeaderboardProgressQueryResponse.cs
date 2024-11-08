using Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Get;

public class GetLeaderboardProgressQueryResponse : Response<PagedResponse<LeaderboardProgressModel>>;