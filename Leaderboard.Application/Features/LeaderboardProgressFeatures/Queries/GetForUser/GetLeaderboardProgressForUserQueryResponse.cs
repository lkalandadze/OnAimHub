using Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.GetForUser;

public class GetLeaderboardProgressForUserQueryResponse : Response<PagedResponse<LeaderboardProgressModel>>
{
    public IEnumerable<LeaderboardProgressModel> Top20Users { get; set; }
    public LeaderboardProgressModel CurrentUser { get; set; }
};