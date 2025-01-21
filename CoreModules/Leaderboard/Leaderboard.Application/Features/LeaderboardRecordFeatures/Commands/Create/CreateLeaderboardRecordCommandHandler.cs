using Leaderboard.Application.Services.Abstract;
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
    private readonly ILeaderboardService _leaderboardService;
    public CreateLeaderboardRecordCommandHandler(ILeaderboardRecordRepository leaderboardRecordRepository, IBackgroundJobScheduler backgroundJobScheduler, IJobService jobService, ILeaderboardScheduleRepository leaderboardScheduleRepository, ILeaderboardService leaderboardService)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _backgroundJobScheduler = backgroundJobScheduler;
        _jobService = jobService;
        _leaderboardScheduleRepository = leaderboardScheduleRepository;
        _leaderboardService = leaderboardService;
    }

    public async Task Handle(CreateLeaderboardRecordCommand request, CancellationToken cancellationToken)
    {
        _leaderboardService.ValidateLeaderboardPrizes(request.LeaderboardPrizes);

        int nextExternalId = await _leaderboardRecordRepository.GetMaxExternalIdAsync(cancellationToken) + 1;

        var leaderboard = new LeaderboardRecord(
            request.PromotionId,
            nextExternalId,
            request.PromotionName,
            request.Title,
            request.Description,
            request.EventType,
            request.AnnouncementDate.ToUniversalTime(),
            request.StartDate.ToUniversalTime(),
            request.EndDate.ToUniversalTime(),
            request.IsGenerated,
            request.TemplateId,
            request.ScheduleId,
            request.CreatedBy,
            request.CorrelationId
        );


        foreach (var prize in request.LeaderboardPrizes)
        {
            leaderboard.AddLeaderboardRecordPrizes(prize.StartRank, prize.EndRank, prize.CoinId, prize.Amount);
        }

        await _leaderboardRecordRepository.InsertAsync(leaderboard);
        await _leaderboardRecordRepository.SaveChangesAsync(cancellationToken);

        if (request.RepeatType != RepeatType.None)
        {
            // Calculate duration hours
            var announcementDurationHours = (request.StartDate - request.AnnouncementDate).TotalHours;
            var startDurationHours = (request.EndDate - request.StartDate).TotalHours;
            var endDurationHours = (request.EndDate - request.AnnouncementDate).TotalHours;

            var schedule = new LeaderboardSchedule(
                request.Title,
                request.RepeatType,
                request.RepeatValue,
                leaderboard.Id,
                (int)announcementDurationHours,
                (int)startDurationHours,
                (int)endDurationHours
            );

            await _leaderboardScheduleRepository.InsertAsync(schedule);
            await _leaderboardScheduleRepository.SaveChangesAsync(cancellationToken);

            _backgroundJobScheduler.ScheduleJob(schedule);
        }
    }
}