using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace Leaderboard.Infrastructure.Repositories;

public class LeaderboardProgressRepository : ILeaderboardProgressRepository
{
    private readonly IDatabase _redisDatabase;

    public LeaderboardProgressRepository(IConnectionMultiplexer redisConnection)
    {
        _redisDatabase = redisConnection.GetDatabase();
    }

    public async Task SaveProgressAsync(LeaderboardProgress progress, TimeSpan expiry, CancellationToken cancellationToken)
    {
        var key = $"LeaderboardProgress:{progress.LeaderboardRecordId}:{progress.PlayerId}";
        var data = JsonSerializer.Serialize(progress);
        await _redisDatabase.StringSetAsync(key, data, expiry);
    }

    public async Task<LeaderboardProgress?> GetProgressAsync(int playerId, int leaderboardRecordId, CancellationToken cancellationToken)
    {
        var key = $"LeaderboardProgress:{leaderboardRecordId}:{playerId}";
        var data = await _redisDatabase.StringGetAsync(key);

        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<LeaderboardProgress>(data!);
    }

    public async Task<IEnumerable<LeaderboardProgress>> GetAllProgressAsync(int leaderboardRecordId, CancellationToken cancellationToken)
    {
        var server = _redisDatabase.Multiplexer.GetServer(_redisDatabase.Multiplexer.GetEndPoints().First());
        var keys = server.Keys(pattern: $"LeaderboardProgress:{leaderboardRecordId}:*");

        var progressList = new List<LeaderboardProgress>();
        foreach (var key in keys)
        {
            var data = await _redisDatabase.StringGetAsync(key);
            if (!data.IsNullOrEmpty)
            {
                progressList.Add(JsonSerializer.Deserialize<LeaderboardProgress>(data!)!);
            }
        }

        return progressList;
    }

    public async Task<IEnumerable<LeaderboardProgress>> GetUserAllActiveProgressesAsync(int playerId, CancellationToken cancellationToken)
    {
        var server = _redisDatabase.Multiplexer.GetServer(_redisDatabase.Multiplexer.GetEndPoints().First());

        var pattern = $"LeaderboardProgress:*:{playerId}";
        var keys = server.Keys(pattern: pattern, pageSize: 1000, database: _redisDatabase.Database);

        var progressList = new List<LeaderboardProgress>();

        foreach (var key in keys)
        {
            var data = await _redisDatabase.StringGetAsync(key);
            if (!data.IsNullOrEmpty)
            {
                var progress = JsonSerializer.Deserialize<LeaderboardProgress>(data);

                if (progress != null)
                    progressList.Add(progress);
            }
        }

        return progressList;
    }

    public async Task<int> GetPlacementAsync(int leaderboardRecordId, int playerId, CancellationToken cancellationToken)
    {
        var allProgressRecords = await GetAllProgressAsync(leaderboardRecordId, cancellationToken);

        var sortedProgressRecords = allProgressRecords
            .OrderByDescending(x => x.Amount)
            .ToList();

        var placement = sortedProgressRecords
            .Select((progress, index) => new { progress.PlayerId, Placement = index + 1 })
            .FirstOrDefault(x => x.PlayerId == playerId);

        return placement?.Placement ?? 0;
    }

    public async Task DeleteProgressAsync(int playerId, int leaderboardRecordId, CancellationToken cancellationToken)
    {
        var key = $"LeaderboardProgress:{leaderboardRecordId}:{playerId}";
        await _redisDatabase.KeyDeleteAsync(key);
    }

    public async Task ClearLeaderboardProgressAsync(int leaderboardRecordId, CancellationToken cancellationToken)
    {
        var server = _redisDatabase.Multiplexer.GetServer(_redisDatabase.Multiplexer.GetEndPoints().First());
        var keys = server.Keys(pattern: $"LeaderboardProgress:{leaderboardRecordId}:*");

        foreach (var key in keys)
        {
            await _redisDatabase.KeyDeleteAsync(key);
        }
    }
}