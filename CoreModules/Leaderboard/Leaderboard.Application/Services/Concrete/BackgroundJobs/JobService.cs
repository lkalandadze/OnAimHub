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
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly ILeaderboardResultRepository _leaderboardResultRepository;
    private readonly ILeaderboardScheduleRepository _leaderboardScheduleRepository;
    private readonly IMediator _mediator;
    public JobService(ILeaderboardRecordRepository leaderboardRecordRepository,
                     ILeaderboardProgressRepository leaderboardProgressRepository,
                     ILeaderboardResultRepository leaderboardResultRepository,
                     ILeaderboardScheduleRepository leaderboardScheduleRepository,
                     IMediator mediator)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _leaderboardResultRepository = leaderboardResultRepository;
        _leaderboardScheduleRepository = leaderboardScheduleRepository;
        _mediator = mediator;
    }

    public async Task<List<LeaderboardRecord>> GetAllJobsAsync()
    {
        return await _leaderboardRecordRepository.QueryAsync();
    }

    public async Task<List<LeaderboardSchedule>> GetAllActiveSchedulesAsync()
    {
        return await _leaderboardScheduleRepository.QueryAsync(s => s.Status == LeaderboardScheduleStatus.Active);
    }

    public async Task ExecuteLeaderboardRecordGeneration(int scheduleId)
    {
        var schedule = await _leaderboardScheduleRepository.Query()
            .Include(s => s.LeaderboardRecord)
            .ThenInclude(p => p.LeaderboardRecordPrizes)
            .FirstOrDefaultAsync(s => s.Id == scheduleId);

        if (schedule == null)
            throw new Exception("Leaderboard schedule not found.");

        var leaderboardRecord = schedule.LeaderboardRecord;

        if (leaderboardRecord == null)
            throw new Exception("Associated leaderboard record not found.");

        var now = DateTimeOffset.UtcNow;

        // Calculate the first start date based on the schedule's repeat type
        var nextStartDate = CalculateNextStartDate(leaderboardRecord.StartDate, leaderboardRecord, schedule);

        // Ensure the calculated start date is in the future
        if (nextStartDate <= now)
        {
            nextStartDate = CalculateNextInterval(nextStartDate, schedule);
        }

        while (nextStartDate <= leaderboardRecord.EndDate)
        {
            // Calculate the duration of the leaderboard
            var durationDays = (leaderboardRecord.EndDate - leaderboardRecord.StartDate).Days;

            // Calculate the corresponding end date
            var endDate = nextStartDate.AddDays(durationDays);

            // Calculate the announcement date
            var announcementDate = nextStartDate.AddDays(-leaderboardRecord.StartDate.Subtract(leaderboardRecord.AnnouncementDate).Days);

            // Create a new leaderboard record
            var newRecord = new LeaderboardRecord(
                leaderboardRecord.PromotionId,
                leaderboardRecord.Title,
                leaderboardRecord.Description,
                leaderboardRecord.EventType,
                announcementDate.ToUniversalTime(),
                nextStartDate.ToUniversalTime(),
                endDate.ToUniversalTime(),
                true,
                leaderboardRecord.TemplateId,
                scheduleId,
                null
            );

            // Copy prizes from the original record
            foreach (var prize in leaderboardRecord.LeaderboardRecordPrizes)
            {
                newRecord.AddLeaderboardRecordPrizes(prize.StartRank, prize.EndRank, prize.CoinId, prize.Amount);
            }

            await _leaderboardRecordRepository.InsertAsync(newRecord);

            // Move to the next interval
            nextStartDate = CalculateNextInterval(nextStartDate, schedule);
        }

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

    //public async Task UpdateScheduleStatusesAsync()
    //{
    //    var now = DateTimeOffset.UtcNow;

    //    var activeSchedules = await _leaderboardScheduleRepository.QueryAsync(s =>
    //        s.Status == LeaderboardScheduleStatus.Active && s.EndDate <= now);

    //    foreach (var schedule in activeSchedules)
    //        schedule.UpdateStatus(LeaderboardScheduleStatus.Completed);

    //    await _leaderboardScheduleRepository.SaveChangesAsync();
    //}

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
        // Fetch the leaderboard record from the database
        var leaderboardRecord = await _leaderboardRecordRepository.Query()
            .FirstOrDefaultAsync(x => x.Id == leaderboardRecordId);

        if (leaderboardRecord == null || leaderboardRecord.Status != LeaderboardRecordStatus.Finished)
        {
            throw new Exception($"Leaderboard record with ID {leaderboardRecordId} is not finished.");
        }

        // Fetch all progress from Redis
        var leaderboardProgress = await _leaderboardProgressRepository.GetAllProgressAsync(leaderboardRecordId, CancellationToken.None);

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

        await _leaderboardProgressRepository.ClearLeaderboardProgressAsync(leaderboardRecordId, CancellationToken.None);
    }

    private DateTimeOffset CalculateNextStartDate(DateTimeOffset startDate, LeaderboardRecord record, LeaderboardSchedule schedule)
    {
        var startTime = record.StartDate.TimeOfDay;

        return schedule.RepeatType switch
        {
            RepeatType.EveryNDays => startDate.AddDays(schedule.RepeatValue ?? 1).Add(startTime),

            RepeatType.DayOfWeek =>
                startDate.DayOfWeek == (DayOfWeek)(schedule.RepeatValue ?? 0)
                    ? startDate.Date.Add(startTime) // Start on the same day if it matches
                    : startDate.Date.AddDays(((schedule.RepeatValue ?? 0) - (int)startDate.DayOfWeek + 7) % 7).Add(startTime),

            RepeatType.DayOfMonth =>
                startDate.Day <= (schedule.RepeatValue ?? 1)
                    ? new DateTimeOffset(startDate.Year, startDate.Month, schedule.RepeatValue ?? 1, startTime.Hours, startTime.Minutes, startTime.Seconds, startDate.Offset)
                    : new DateTimeOffset(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, schedule.RepeatValue ?? 1, startTime.Hours, startTime.Minutes, startTime.Seconds, startDate.Offset),

            _ => throw new ArgumentException("Unsupported RepeatType for schedule.")
        };
    }

    private DateTimeOffset CalculateNextInterval(DateTimeOffset current, LeaderboardSchedule schedule)
    {
        return schedule.RepeatType switch
        {
            RepeatType.EveryNDays => current.AddDays(schedule.RepeatValue ?? 1),

            RepeatType.DayOfWeek => current.AddDays(7), // Move to the same day next week

            RepeatType.DayOfMonth => current.AddMonths(1), // Move to the same day next month

            _ => throw new ArgumentException("Unsupported RepeatType for interval calculation.")
        };
    }
}