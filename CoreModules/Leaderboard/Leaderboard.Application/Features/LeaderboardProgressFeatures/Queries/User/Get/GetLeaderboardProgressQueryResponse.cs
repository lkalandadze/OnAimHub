using Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.User.Get;

public class GetLeaderboardProgressQueryResponse : Response<PagedResponse<LeaderboardProgressModel>>;