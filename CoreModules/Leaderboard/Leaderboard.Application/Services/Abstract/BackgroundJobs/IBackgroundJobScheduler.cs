using Leaderboard.Domain.Entities;

namespace Leaderboard.Application.Services.Abstract.BackgroundJobs;

public interface IBackgroundJobScheduler
{
    void ScheduleJob(LeaderboardSchedule schedule);
    //void ScheduleRecordJob(LeaderboardRecord job);
    void RemoveScheduledJob(int jobId);
    void ExecuteJob(int scheduleId);
    string GenerateCronExpression(LeaderboardSchedule schedule);
}