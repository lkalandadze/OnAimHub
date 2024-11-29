using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using Leaderboard.Infrastructure.Repositories;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Create;

public class CreateLeaderboardRecordCommandHandler : IRequestHandler<CreateLeaderboardRecordCommand>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    private readonly ILeaderboardScheduleRepository _leaderboardScheduleRepository;
    private readonly IBackgroundJobScheduler _backgroundJobScheduler;
    private readonly IJobService _jobService;
    public CreateLeaderboardRecordCommandHandler(ILeaderboardRecordRepository leaderboardRecordRepository, IBackgroundJobScheduler backgroundJobScheduler, IJobService jobService, ILeaderboardScheduleRepository leaderboardScheduleRepository)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _backgroundJobScheduler = backgroundJobScheduler;
        _jobService = jobService;
        _leaderboardScheduleRepository = leaderboardScheduleRepository;
    }

    public async Task Handle(CreateLeaderboardRecordCommand request, CancellationToken cancellationToken)
    {
        // Create the LeaderboardRecord
        var leaderboard = new LeaderboardRecord(
            request.PromotionId,
            request.Title,
            request.Description,
            request.EventType,
            request.AnnouncementDate.ToUniversalTime(),
            request.StartDate.ToUniversalTime(),
            request.EndDate.ToUniversalTime(),
            request.IsGenerated,
            request.TemplateId,
            request.ScheduleId,
            request.CorrelationId
        );

        // Add prizes
        foreach (var prize in request.LeaderboardPrizes)
        {
            leaderboard.AddLeaderboardRecordPrizes(prize.StartRank, prize.EndRank, prize.CoinId, prize.Amount);
        }

        await _leaderboardRecordRepository.InsertAsync(leaderboard);
        await _leaderboardRecordRepository.SaveChangesAsync(cancellationToken);

        // Schedule job only for applicable RepeatTypes
        if (request.RepeatType != RepeatType.None)
        {
            var schedule = new LeaderboardSchedule(
                request.Title,
                request.RepeatType,
                request.RepeatValue,
                leaderboard.Id
            );

            await _leaderboardScheduleRepository.InsertAsync(schedule);
            await _leaderboardScheduleRepository.SaveChangesAsync(cancellationToken);

            // Schedule the job using the BackgroundJobScheduler
            _backgroundJobScheduler.ScheduleJob(schedule);
        }
    }
}