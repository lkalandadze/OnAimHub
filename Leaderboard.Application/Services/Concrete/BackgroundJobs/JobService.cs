﻿using Leaderboard.Application.Features.LeaderboardTemplateFeatures.Queries.GetById;
using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Application.Services.Concrete.BackgroundJobs;

public class JobService : IJobService
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly ILeaderboardResultRepository _leaderboardResultRepository;
    private readonly IMediator _mediator;
    public JobService(ILeaderboardRecordRepository leaderboardRecordRepository,
                     ILeaderboardTemplateRepository leaderboardTemplateRepository,
                     ILeaderboardProgressRepository leaderboardProgressRepository,
                     ILeaderboardResultRepository leaderboardResultRepository,
                     IMediator mediator)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _leaderboardResultRepository = leaderboardResultRepository;
        _mediator = mediator;
    }

    public async Task<List<LeaderboardRecord>> GetAllJobsAsync()
    {
        return await _leaderboardRecordRepository.QueryAsync();
    }

    public async Task<List<LeaderboardTemplate>> GetAllTemplateJobsAsync()
    {
        return await _leaderboardTemplateRepository.QueryAsync();
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
            LeaderboardType.Win, // needs to be dynamic
            leaderboardTemplate.JobType,
            templateId,
            LeaderboardRecordStatus.Created,
            true
        );

        foreach (var prize in leaderboardTemplate.Prizes)
        {
            newRecord.AddLeaderboardRecordPrizes(prize.StartRank, prize.EndRank, prize.PrizeId, prize.Amount);
        }

        await _leaderboardRecordRepository.InsertAsync(newRecord);
        await _leaderboardRecordRepository.SaveChangesAsync();
    }

    public async Task UpdateLeaderboardRecordStatusAsync()
    {
        var records = await _leaderboardRecordRepository.QueryAsync();
        var now = DateTimeOffset.UtcNow;

        foreach (var record in records)
        {
            if (now >= record.StartDate && now < record.EndDate && record.Status != LeaderboardRecordStatus.InProgress)
            {
                record.Status = LeaderboardRecordStatus.InProgress;
            }
            else if (now >= record.EndDate && record.Status != LeaderboardRecordStatus.Finished)
            {
                record.Status = LeaderboardRecordStatus.Finished;

                // Schedule processing of leaderboard results when the leaderboard finishes
                await ProcessLeaderboardResults(record.Id);
            }
            else if (now >= record.AnnouncementDate && now < record.StartDate && record.Status != LeaderboardRecordStatus.Announced)
            {
                record.Status = LeaderboardRecordStatus.Announced;
            }
            else if (now < record.AnnouncementDate && record.Status != LeaderboardRecordStatus.Created)
            {
                record.Status = LeaderboardRecordStatus.Created;
            }
        }

        await _leaderboardRecordRepository.SaveChangesAsync();
    }

    public async Task ExecuteLeaderboardJob(int leaderboardRecordId)
    {
        // Fetch the LeaderboardRecord
        var leaderboardRecord = _leaderboardRecordRepository.Query().FirstOrDefault(x => x.Id == leaderboardRecordId);

        if (leaderboardRecord == null)
        {
            throw new Exception($"Leaderboard record with ID {leaderboardRecordId} not found.");
        }

        // Update status or perform necessary logic
        if (leaderboardRecord.Status == LeaderboardRecordStatus.Created)
        {
            leaderboardRecord.Status = LeaderboardRecordStatus.InProgress;
            // Additional logic to handle InProgress state (e.g., calculate scores, process events)
        }

        if (leaderboardRecord.Status == LeaderboardRecordStatus.InProgress && leaderboardRecord.EndDate <= DateTimeOffset.UtcNow)
        {
            leaderboardRecord.Status = LeaderboardRecordStatus.Finished;
            // Additional logic to handle Finished state (e.g., award prizes)
        }

        // Save the updated record
        _leaderboardRecordRepository.Update(leaderboardRecord);
        await _leaderboardRecordRepository.SaveChangesAsync();
    }

    public async Task ProcessLeaderboardResults(int leaderboardRecordId)
    {
        var leaderboardRecord = await _leaderboardRecordRepository.Query()
            .FirstOrDefaultAsync(x => x.Id == leaderboardRecordId);

        if (leaderboardRecord == null || leaderboardRecord.Status != LeaderboardRecordStatus.Finished)
        {
            throw new Exception($"Leaderboard record with ID {leaderboardRecordId} is not finished.");
        }

        var leaderboardProgress = await _leaderboardProgressRepository.Query()
            .Where(x => x.LeaderboardRecordId == leaderboardRecordId)
            .ToListAsync();

        if (!leaderboardProgress.Any())
        {
            throw new Exception("No progress found for this leaderboard.");
        }

        var sortedProgress = leaderboardProgress
            .OrderByDescending(x => x.Amount)
            .ToList();

        int placement = 1;
        foreach (var progress in sortedProgress)
        {
            var leaderboardResult = new LeaderboardResult(
                leaderboardRecordId,
                progress.PlayerId,
                progress.PlayerUsername,
                placement++,
                progress.Amount
            );

            await _leaderboardResultRepository.InsertAsync(leaderboardResult);
        }

        await _leaderboardResultRepository.SaveChangesAsync();

        foreach (var progress in leaderboardProgress)
        {
            _leaderboardProgressRepository.Delete(progress);
        }

        await _leaderboardProgressRepository.SaveChangesAsync();
    }
}