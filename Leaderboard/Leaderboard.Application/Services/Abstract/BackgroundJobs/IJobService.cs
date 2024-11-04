using Leaderboard.Domain.Entities;

namespace Leaderboard.Application.Services.Abstract.BackgroundJobs;

public interface IJobService
{
    Task<List<LeaderboardRecord>> GetAllJobsAsync();
    Task<List<LeaderboardTemplate>> GetAllTemplateJobsAsync();
    Task<List<LeaderboardSchedule>> GetAllActiveSchedulesAsync();
    Task ExecuteLeaderboardRecordGeneration(int templateId);
    Task UpdateLeaderboardRecordStatusAsync();
    Task UpdateScheduleStatusesAsync();
    Task ExecuteLeaderboardJob(int leaderboardRecordId);
    Task ProcessLeaderboardResults(int leaderboardRecordId);
}