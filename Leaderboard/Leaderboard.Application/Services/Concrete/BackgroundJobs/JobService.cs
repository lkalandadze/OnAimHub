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
    private readonly ILeaderboardScheduleRepository _leaderboardScheduleRepository;
    private readonly IMediator _mediator;
    public JobService(ILeaderboardRecordRepository leaderboardRecordRepository,
                     ILeaderboardTemplateRepository leaderboardTemplateRepository,
                     ILeaderboardProgressRepository leaderboardProgressRepository,
                     ILeaderboardResultRepository leaderboardResultRepository,
                     ILeaderboardScheduleRepository leaderboardScheduleRepository,
                     IMediator mediator)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _leaderboardResultRepository = leaderboardResultRepository;
        _leaderboardScheduleRepository = leaderboardScheduleRepository;
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

    public async Task<List<LeaderboardSchedule>> GetAllActiveSchedulesAsync()
    {
        return await _leaderboardScheduleRepository.QueryAsync(s => s.Status == LeaderboardScheduleStatus.Active);
    }

    public async Task ExecuteLeaderboardRecordGeneration(int scheduleId)
     {
        //var query = new GetLeaderboardTemplateByIdQuery(templateId);

        var schedule = await _leaderboardScheduleRepository.Query()
            .Include(s => s.LeaderboardTemplate) // Include the related LeaderboardTemplate for prize data
            .FirstOrDefaultAsync(s => s.Id == scheduleId);

        if (schedule == default)
            throw new Exception("Leaderboard schedule not found");

        var leaderboardTemplate = schedule.LeaderboardTemplate;

        if (leaderboardTemplate == null)
            throw new Exception("Associated leaderboard template not found");

        var now = DateTimeOffset.UtcNow;
        DateTimeOffset creationDate, announcementDate, startDate, endDate;

        switch (schedule.RepeatType)
        {
            case RepeatType.SingleDate:
                if (!schedule.SpecificDate.HasValue)
                    throw new Exception("SpecificDate is required for RepeatType.SingleDate");

                startDate = now.Date.Add(schedule.StartTime);
                endDate = startDate.AddDays(leaderboardTemplate.EndIn);
                break;

            case RepeatType.EveryNDays:
                if (!schedule.RepeatValue.HasValue)
                    throw new Exception("RepeatValue is required for RepeatType.EveryNDays");

                startDate = now.Date.AddDays(schedule.RepeatValue.Value).Add(schedule.StartDate.TimeOfDay);
                endDate = startDate.AddDays(leaderboardTemplate.EndIn);
                break;

            case RepeatType.DayOfWeek:
                if (!schedule.RepeatValue.HasValue)
                    throw new Exception("RepeatValue is required for RepeatType.DayOfWeek");

                int daysUntilTargetDay = ((schedule.RepeatValue.Value - (int)now.DayOfWeek + 7) % 7);
                startDate = now.Date.AddDays(daysUntilTargetDay).Add(schedule.StartDate.TimeOfDay);
                endDate = startDate.AddDays(leaderboardTemplate.EndIn);
                break;

            case RepeatType.DayOfMonth:
                if (!schedule.RepeatValue.HasValue)
                    throw new Exception("RepeatValue is required for RepeatType.DayOfMonth");

                var targetDay = schedule.RepeatValue.Value;
                var currentMonth = new DateTime(now.Year, now.Month, 1);
                var nextScheduledDate = currentMonth.AddMonths(1).AddDays(targetDay - 1);
                startDate = new DateTimeOffset(nextScheduledDate.Add(schedule.StartDate.TimeOfDay), TimeSpan.Zero);
                endDate = startDate.AddDays(leaderboardTemplate.EndIn);
                break;

            default:
                throw new ArgumentException("Unsupported repeat type");
        }

        // Calculate creation and announcement dates
        creationDate = startDate.AddDays(-leaderboardTemplate.AnnounceIn);
        announcementDate = startDate.AddDays(-leaderboardTemplate.StartIn);

        // Create a new LeaderboardRecord based on the calculated dates and template info
        var newRecord = new LeaderboardRecord(
            schedule.Name,
            creationDate.ToUniversalTime(),
            announcementDate.ToUniversalTime(),
            startDate.ToUniversalTime(),
            endDate.ToUniversalTime(),
            LeaderboardType.Win, // Set dynamically if needed
            //schedule.RepeatType,
            schedule.LeaderboardTemplateId,
            LeaderboardRecordStatus.Created,
            true
        );

        foreach (var prize in leaderboardTemplate.LeaderboardTemplatePrizes)
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

    public async Task UpdateScheduleStatusesAsync()
    {
        var now = DateTimeOffset.UtcNow;

        var activeSchedules = await _leaderboardScheduleRepository.QueryAsync(s =>
            s.Status == LeaderboardScheduleStatus.Active && s.EndDate <= now);

        foreach (var schedule in activeSchedules)
        {
            schedule.UpdateStatus(LeaderboardScheduleStatus.Completed);
        }

        await _leaderboardScheduleRepository.SaveChangesAsync();
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