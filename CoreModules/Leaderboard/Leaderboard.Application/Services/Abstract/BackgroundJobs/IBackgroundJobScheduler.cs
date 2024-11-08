using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Services.Abstract.BackgroundJobs;

public interface IBackgroundJobScheduler
{
    void ScheduleJob(LeaderboardSchedule schedule);
    //void ScheduleRecordJob(LeaderboardRecord job);
    void RemoveScheduledJob(int jobId);
    void ExecuteJob(int leaderboardTemplateId);
    string GenerateCronExpression(LeaderboardSchedule schedule);
}