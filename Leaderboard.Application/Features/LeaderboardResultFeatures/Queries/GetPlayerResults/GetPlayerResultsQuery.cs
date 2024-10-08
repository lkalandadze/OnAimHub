using MediatR;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.GetPlayerResults;

public class GetPlayerResultsQuery : PagedRequest, IRequest<GetPlayerResultsQueryResponse>
{
    public int PlayerId { get; }
}