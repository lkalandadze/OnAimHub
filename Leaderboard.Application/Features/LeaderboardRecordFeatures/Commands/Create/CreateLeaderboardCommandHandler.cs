using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Create;

public class CreateLeaderboardCommandHandler : IRequestHandler<CreateLeaderboardCommand>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    public CreateLeaderboardCommandHandler(ILeaderboardRecordRepository leaderboardRecordRepository)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
    }

    public async Task Handle(CreateLeaderboardCommand request, CancellationToken cancellationToken)
    {
        var leaderboard = new LeaderboardRecord(
            request.Name,
            request.AnnouncementDate,
            request.StartDate,
            request.EndDate,
            request.LeaderboardType,
            request.JobType,
            request.LeaderboardTemplateId);

        foreach (var prize in request.LeaderboardPrizes)
        {
            leaderboard.AddLeaderboardPrizes(prize.StartRank, prize.EndRank, prize.PrizeId, prize.Amount);
        }

        await _leaderboardRecordRepository.InsertAsync(leaderboard);
        await _leaderboardRecordRepository.SaveChangesAsync(cancellationToken);
    }
}