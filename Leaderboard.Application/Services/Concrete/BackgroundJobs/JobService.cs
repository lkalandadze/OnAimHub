using Leaderboard.Application.Features.LeaderboardTemplateFeatures.Queries.GetById;
using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using MediatR;

namespace Leaderboard.Application.Services.Concrete.BackgroundJobs;

public class JobService : IJobService
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    private readonly IMediator _mediator;
    public JobService(ILeaderboardRecordRepository leaderboardRecordRepository, IMediator mediator)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _mediator = mediator;
    }

    public async Task<List<LeaderboardRecord>> GetAllJobsAsync()
    {
        return await _leaderboardRecordRepository.QueryAsync();
    }

    public async Task ExecuteLeaderboardRecordGeneration(int templateId)
     {
        var query = new GetLeaderboardTemplateByIdQuery(templateId);
        var response = await _mediator.Send(query);

        if (response?.Data == null)
        {
            throw new Exception("Leaderboard template not found");
        }

        var leaderboardTemplate = response.Data;

        var now = DateTimeOffset.UtcNow;
        DateTimeOffset creationDate, announcementDate, startDate, endDate;

        switch (leaderboardTemplate.JobType)
        {
            case JobTypeEnum.Daily:
                startDate = now.Date.Add(leaderboardTemplate.StartTime).ToUniversalTime();
                endDate = startDate.AddDays(leaderboardTemplate.DurationInDays);
                creationDate = startDate.AddDays(-leaderboardTemplate.CreationLeadTimeInDays);
                announcementDate = startDate.AddDays(-leaderboardTemplate.AnnouncementLeadTimeInDays);
                break;

            case JobTypeEnum.Weekly:
                // Start on the next Monday at the specified time
                int daysUntilMonday = ((int)DayOfWeek.Monday - (int)now.DayOfWeek + 7) % 7;
                startDate = now.Date.AddDays(daysUntilMonday).Add(leaderboardTemplate.StartTime).ToUniversalTime();
                endDate = startDate.AddDays(leaderboardTemplate.DurationInDays);
                creationDate = startDate.AddDays(-leaderboardTemplate.CreationLeadTimeInDays);
                announcementDate = startDate.AddDays(-leaderboardTemplate.AnnouncementLeadTimeInDays);
                break;

            case JobTypeEnum.Monthly:
                // Start on the first of the next month at the specified time
                var firstOfNextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                startDate = new DateTimeOffset(firstOfNextMonth.Add(leaderboardTemplate.StartTime), TimeSpan.Zero).ToUniversalTime();
                endDate = startDate.AddDays(leaderboardTemplate.DurationInDays);
                creationDate = startDate.AddDays(-leaderboardTemplate.CreationLeadTimeInDays);
                announcementDate = startDate.AddDays(-leaderboardTemplate.AnnouncementLeadTimeInDays);
                break;

            case JobTypeEnum.Custom:
                // Custom logic for start, end, and announcement dates
                startDate = now.Date.Add(leaderboardTemplate.StartTime).ToUniversalTime();
                endDate = startDate.AddDays(leaderboardTemplate.DurationInDays);
                creationDate = startDate.AddDays(-leaderboardTemplate.CreationLeadTimeInDays);
                announcementDate = startDate.AddDays(-leaderboardTemplate.AnnouncementLeadTimeInDays);
                break;

            default:
                throw new ArgumentException("Unsupported job type");
        }

        var newRecord = new LeaderboardRecord(
            leaderboardTemplate.Name,
            creationDate.ToUniversalTime(),
            announcementDate.ToUniversalTime(),
            startDate.ToUniversalTime(),
            endDate.ToUniversalTime(),
            LeaderboardType.Transaction, // needs to be dynamic
            leaderboardTemplate.JobType,
            templateId
        );

        foreach (var prize in leaderboardTemplate.Prizes)
        {
            newRecord.AddLeaderboardRecordPrizes(prize.StartRank, prize.EndRank, prize.PrizeId, prize.Amount);
        }

        await _leaderboardRecordRepository.InsertAsync(newRecord);
        await _leaderboardRecordRepository.SaveChangesAsync();
    }
}