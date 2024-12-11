using Hub.Application.Features.RewardFeatures.Commands.ClaimReward;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using MediatR;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Leaderboard;

namespace Hub.Application.Features.LeaderboardFeatures.Commands.PlayLeaderboard;

public class PlayLeaderboardCommandHandler : IRequestHandler<PlayLeaderboardCommand>
{
    private readonly ITransactionService _transactionService;
    private readonly IAuthService _authService;
    private readonly IMessageBus _messageBus;
    public PlayLeaderboardCommandHandler(ITransactionService transactionService, IAuthService authService, IMessageBus messageBus)
    {
        _transactionService = transactionService;
        _authService = authService;
        _messageBus = messageBus;
    }

    public async Task<Unit> Handle(PlayLeaderboardCommand request, CancellationToken cancellationToken)
    {
        var playerId = _authService.GetCurrentPlayerId();

        var @events = new PlayLeaderboardEvent(Guid.NewGuid(), request.LeaderboardRecordId, request.GeneratedAmount, request.PromotionId, playerId);
        await _messageBus.Publish(@events);

        return Unit.Value;
    }
}
