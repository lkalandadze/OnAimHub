using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Commands.Create;

public class CreateLeaderboardResultsCommandHandler : IRequestHandler<CreateLeaderboardResultsCommand>
{
    private readonly ILeaderboardResultRepository _leaderboardResultRepository;

    public CreateLeaderboardResultsCommandHandler(ILeaderboardResultRepository leaderboardResultRepository)
    {
        _leaderboardResultRepository = leaderboardResultRepository;
    }

    public async Task Handle(CreateLeaderboardResultsCommand request, CancellationToken cancellationToken)
    {
        foreach (var result in request.Results)
        {
            var leaderboardResult = new LeaderboardResult
            {
                LeaderboardRecordId = result.LeaderboardRecordId,
                PlayerId = result.PlayerId,
                PlayerUsername = result.PlayerUsername,
                Placement = result.Placement,
                Amount = result.Amount
            };

            await _leaderboardResultRepository.InsertAsync(leaderboardResult);
        }

        await _leaderboardResultRepository.SaveChangesAsync(cancellationToken);
    }
}