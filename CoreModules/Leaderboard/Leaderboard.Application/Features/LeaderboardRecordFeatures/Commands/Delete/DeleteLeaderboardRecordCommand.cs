using MediatR;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Delete;

public record DeleteLeaderboardRecordCommand(Guid CorrelationId) : IRequest;