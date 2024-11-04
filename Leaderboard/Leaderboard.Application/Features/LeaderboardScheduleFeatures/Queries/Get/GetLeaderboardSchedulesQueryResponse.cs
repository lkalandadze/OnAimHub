using Leaderboard.Application.Features.LeaderboardScheduleFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.Queries.Get;

public class GetLeaderboardSchedulesQueryResponse : Response<PagedResponse<LeaderboardSchedulesModel>>;