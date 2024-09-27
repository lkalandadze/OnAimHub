using Leaderboard.Domain.Entities;
using Leaderboard.Domain.Enum;

namespace Leaderboard.Application.Services.Abstract.BackgroundJobs;

public interface IBackgroundJobScheduler
{
    void ScheduleJob(LeaderboardRecord leaderboardRecord);
    void ScheduleRecordJob(LeaderboardRecord job);
    void RemoveScheduledJob(int jobId);
    void ExecuteJob(int leaderboardTemplateId);
    string GenerateCronExpression(JobTypeEnum jobType);
}