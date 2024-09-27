using MediatR;
using Shared.Lib.Wrappersl;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.GetForUser;

public class GetLeaderboardProgressForUserQuery : PagedRequest, IRequest<GetLeaderboardProgressForUserQueryResponse>
{
    public int LeaderboardRecordId { get; set; }
    public int PlayerId { get; set; }
}