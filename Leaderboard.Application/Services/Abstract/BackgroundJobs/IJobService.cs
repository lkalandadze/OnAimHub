using Leaderboard.Domain.Entities;

namespace Leaderboard.Application.Services.Abstract.BackgroundJobs;

public interface IJobService
{
    Task<List<LeaderboardRecord>> GetAllJobsAsync();
    Task ExecuteLeaderboardRecordGeneration(int templateId);
    Task UpdateLeaderboardRecordStatusAsync();
    Task ExecuteLeaderboardJob(int leaderboardRecordId);
}