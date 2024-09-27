using MediatR;
using Shared.Lib.Wrappersl;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.Get;

public class GetLeaderboardRecordsQuery : PagedRequest, IRequest<GetLeaderboardRecordsQueryResponse>;