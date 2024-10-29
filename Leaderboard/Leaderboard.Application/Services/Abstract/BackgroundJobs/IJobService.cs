using Leaderboard.Domain.Entities;

namespace Leaderboard.Application.Services.Abstract.BackgroundJobs;

public interface IJobService
{
    Task<List<LeaderboardRecord>> GetAllJobsAsync();
    Task<List<LeaderboardTemplate>> GetAllTemplateJobsAsync();
    Task ExecuteLeaderboardRecordGeneration(int templateId);
    Task UpdateLeaderboardRecordStatusAsync();
    Task ExecuteLeaderboardJob(int leaderboardRecordId);
    Task ProcessLeaderboardResults(int leaderboardRecordId);
}