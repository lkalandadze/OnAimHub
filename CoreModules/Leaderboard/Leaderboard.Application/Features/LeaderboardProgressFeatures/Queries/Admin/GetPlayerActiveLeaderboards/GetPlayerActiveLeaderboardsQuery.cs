using MediatR;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Admin.GetPlayerActiveLeaderboards;

public class GetPlayerActiveLeaderboardsQuery : PagedRequest, IRequest<GetPlayerActiveLeaderboardsQueryResponse>
{
    public int PlayerId { get; set; }
}