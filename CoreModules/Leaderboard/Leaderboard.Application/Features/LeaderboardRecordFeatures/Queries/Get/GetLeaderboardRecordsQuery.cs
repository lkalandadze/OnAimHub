using MediatR;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.Get;

public class GetLeaderboardRecordsQuery : PagedRequest, IRequest<GetLeaderboardRecordsQueryResponse>;