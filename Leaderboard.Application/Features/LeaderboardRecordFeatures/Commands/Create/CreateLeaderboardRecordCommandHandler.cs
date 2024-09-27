using Hangfire;
using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Create;

public class CreateLeaderboardRecordCommandHandler : IRequestHandler<CreateLeaderboardRecordCommand>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    private readonly IBackgroundJobScheduler _backgroundJobScheduler;
    private readonly IJobService _jobService;
    public CreateLeaderboardRecordCommandHandler(ILeaderboardRecordRepository leaderboardRecordRepository, IBackgroundJobScheduler backgroundJobScheduler, IJobService jobService)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _backgroundJobScheduler = backgroundJobScheduler;
        _jobService = jobService;
    }

    public async Task Handle(CreateLeaderboardRecordCommand request, CancellationToken cancellationToken)
    {
        var leaderboard = new LeaderboardRecord(
            request.Name,
            request.CreationDate,
            request.AnnouncementDate,
            request.StartDate,
            request.EndDate,
            request.LeaderboardType,
            request.JobType,
            request.LeaderboardTemplateId,
            request.Status,
            false
            );

        foreach (var prize in request.LeaderboardPrizes)
        {
            leaderboard.AddLeaderboardRecordPrizes(prize.StartRank, prize.EndRank, prize.PrizeId, prize.Amount);
        }

        await _leaderboardRecordRepository.InsertAsync(leaderboard);
        await _leaderboardRecordRepository.SaveChangesAsync(cancellationToken);

        _backgroundJobScheduler.ScheduleRecordJob(leaderboard);
    }
}