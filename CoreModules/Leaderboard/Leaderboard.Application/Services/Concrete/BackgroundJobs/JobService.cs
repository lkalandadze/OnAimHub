using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Reward.Leaderboard;

namespace Leaderboard.Application.Services.Concrete.BackgroundJobs;

public class JobService : IJobService
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly ILeaderboardResultRepository _leaderboardResultRepository;
    private readonly ILeaderboardScheduleRepository _leaderboardScheduleRepository;
    private readonly IMediator _mediator;
    private readonly IMessageBus _messageBus;
    public JobService(ILeaderboardRecordRepository leaderboardRecordRepository,
                     ILeaderboardProgressRepository leaderboardProgressRepository,
                     ILeaderboardResultRepository leaderboardResultRepository,
                     ILeaderboardScheduleRepository leaderboardScheduleRepository,
                     IMediator mediator,
                     IMessageBus messageBus)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _leaderboardResultRepository = leaderboardResultRepository;
        _leaderboardScheduleRepository = leaderboardScheduleRepository;
        _mediator = mediator;
        _messageBus = messageBus;
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

        var now = DateTimeOffset.UtcNow;

        try
        {
            // Calculate the first set of dates
            var (nextAnnouncementDate, nextStartDate, nextEndDate) = schedule.CalculateNextDates(now);

            // Validate that the calculated dates are within bounds
            if (nextStartDate > schedule.CalculateEndDate(now) || nextEndDate > DateTimeOffset.MaxValue)
            {
                Console.WriteLine("Calculated dates are out of range. Stopping.");
                return;
            }

            // Create the record
            var newRecord = new LeaderboardRecord(
                schedule.LeaderboardRecord.PromotionId,
                schedule.LeaderboardRecord.ExternalId,
                schedule.LeaderboardRecord.PromotionName,
                schedule.LeaderboardRecord.Title,
                schedule.LeaderboardRecord.Description,
                schedule.LeaderboardRecord.EventType,
                nextAnnouncementDate.ToUniversalTime(),
                nextStartDate.ToUniversalTime(),
                nextEndDate.ToUniversalTime(),
                true,
                schedule.LeaderboardRecord.TemplateId,
                scheduleId,
                null,
                null
            );

            // Add prizes to the record
            foreach (var prize in schedule.LeaderboardRecord.LeaderboardRecordPrizes)
            {
                newRecord.AddLeaderboardRecordPrizes(prize.StartRank, prize.EndRank, prize.CoinId, prize.Amount);
            }

            // Save the record
            try
            {
                await _leaderboardRecordRepository.InsertAsync(newRecord);
                await _leaderboardRecordRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save leaderboard record: {ex.Message}");
                throw;
            }

            // Progress the interval and handle the next calculation if required
            now = nextStartDate.AddHours(schedule.RepeatValue ?? 24);
            (nextAnnouncementDate, nextStartDate, nextEndDate) = schedule.CalculateNextDates(now);

            Console.WriteLine($"Processed: AnnouncementDate={nextAnnouncementDate}, StartDate={nextStartDate}, EndDate={nextEndDate}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in job execution: {ex.Message}");
            throw;
        }
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
        var leaderboardRecord = await _leaderboardRecordRepository.Query()
            .Include(x => x.LeaderboardRecordPrizes)
            .FirstOrDefaultAsync(x => x.Id == leaderboardRecordId);

        if (leaderboardRecord == null || leaderboardRecord.Status != LeaderboardRecordStatus.Finished)
        {
            throw new Exception($"Leaderboard record with ID {leaderboardRecordId} is not finished.");
        }

        var leaderboardProgress = await _leaderboardProgressRepository.GetAllProgressAsync(leaderboardRecordId, CancellationToken.None);

        //if (!leaderboardProgress.Any())
        //{
        //    throw new Exception("No progress found for this leaderboard.");
        //}

        var sortedProgress = leaderboardProgress
            .OrderByDescending(x => x.Amount)
            .ToList();

        var rewardDetails = new List<RewardDetail>();
        int placement = 1;

        foreach (var progress in sortedProgress)
        {
            // Find the prize for the current placement
            var prize = leaderboardRecord.LeaderboardRecordPrizes
                .FirstOrDefault(p => placement >= p.StartRank && placement <= p.EndRank);

            if (prize != null)
            {
                rewardDetails.Add(new RewardDetail(
                    progress.PlayerId,
                    prize.CoinId,
                    prize.Amount,
                    leaderboardRecord.PromotionId,
                    leaderboardRecordId,
                    "Leaderboard"
                ));
            }

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

        // Clear the leaderboard progress from Redis
        await _leaderboardProgressRepository.ClearLeaderboardProgressAsync(leaderboardRecordId, CancellationToken.None);

        // Publish the event to RabbitMQ
        var @events = new ReceiveLeaderboardRewardEvent(Guid.NewGuid(), rewardDetails, DateTime.UtcNow);

        await _messageBus.Publish(@events);
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