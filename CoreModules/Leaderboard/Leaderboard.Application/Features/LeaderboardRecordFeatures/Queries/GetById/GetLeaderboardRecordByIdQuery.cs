using MediatR;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetById;

public record GetLeaderboardRecordByIdQuery(int Id) : IRequest<GetLeaderboardRecordByIdQueryResponse>;