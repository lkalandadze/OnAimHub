using MediatR;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.GetForUser;

public class GetLeaderboardProgressForUserQuery : PagedRequest, IRequest<GetLeaderboardProgressForUserQueryResponse>
{
    public int LeaderboardRecordId { get; set; }
}