using MediatR;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Commands.Delete;

public record DeleteLeaderboardTemplateCommand(Guid CorrelationId) : IRequest;