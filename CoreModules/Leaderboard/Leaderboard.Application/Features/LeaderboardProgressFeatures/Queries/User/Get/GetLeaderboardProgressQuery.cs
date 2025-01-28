using MediatR;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.User.Get;

public class GetLeaderboardProgressQuery : PagedRequest, IRequest<GetLeaderboardProgressQueryResponse>
{
    public int LeaderboardRecordId { get; set; }
}