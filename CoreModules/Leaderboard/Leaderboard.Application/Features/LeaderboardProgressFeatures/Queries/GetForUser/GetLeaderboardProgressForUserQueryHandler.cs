using Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;
using Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Get;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text.Json;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.GetForUser;

public class GetLeaderboardProgressForUserQueryHandler : IRequestHandler<GetLeaderboardProgressForUserQuery, GetLeaderboardProgressForUserQueryResponse>
{
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDatabase _cache;

    public GetLeaderboardProgressForUserQueryHandler(
        ILeaderboardProgressRepository leaderboardProgressRepository,
        IHttpContextAccessor httpContextAccessor,
        IConnectionMultiplexer redisConnection)
    {
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _httpContextAccessor = httpContextAccessor;
        _cache = redisConnection.GetDatabase();
    }

    public async Task<GetLeaderboardProgressForUserQueryResponse> Handle(GetLeaderboardProgressForUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("PlayerId");
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated.");

        int currentUserId = int.Parse(userId);

        // Cache key for the top 10 leaderboard progress
        string cacheKey = $"Leaderboard:{request.LeaderboardRecordId}:Top10";

        // Try to get the cached data
        string? cachedData = await _cache.StringGetAsync(cacheKey);

        List<LeaderboardProgressModel> top10Users;

        if (!string.IsNullOrEmpty(cachedData))
        {
            // Deserialize cached data
            top10Users = JsonSerializer.Deserialize<List<LeaderboardProgressModel>>(cachedData);
        }
        else
        {
            // Fetch all leaderboard progress from Redis
            var allProgress = await _leaderboardProgressRepository.GetAllProgressAsync(request.LeaderboardRecordId, cancellationToken);

            // Get top 10 users
            top10Users = allProgress
                .OrderByDescending(x => x.Amount)
                .Take(10)
                .Select(x => new LeaderboardProgressModel
                {
                    LeaderboardRecordId = x.LeaderboardRecordId,
                    PlayerId = x.PlayerId,
                    PlayerUsername = x.PlayerUsername,
                    Amount = x.Amount
                })
                .ToList();

            // Cache the result with a TTL of 1 minute
            var serializedData = JsonSerializer.Serialize(top10Users);
            await _cache.StringSetAsync(cacheKey, serializedData, TimeSpan.FromMinutes(1));
        }

        // Get current user's progress
        var currentUserProgress = top10Users
            .FirstOrDefault(x => x.PlayerId == currentUserId);

        if (currentUserProgress == null)
        {
            // Fetch current user directly if not in the top 10
            var allProgress = await _leaderboardProgressRepository.GetAllProgressAsync(request.LeaderboardRecordId, cancellationToken);
            var currentUserRecord = allProgress
                .FirstOrDefault(x => x.PlayerId == currentUserId);

            if (currentUserRecord != null)
            {
                currentUserProgress = new LeaderboardProgressModel
                {
                    PlayerId = currentUserRecord.PlayerId,
                    PlayerUsername = currentUserRecord.PlayerUsername,
                    Amount = currentUserRecord.Amount
                };

                // Include the current user in the data if they're not in the top 10
                top10Users.Add(currentUserProgress);
            }
        }

        // Paginate the top 10 (or top 11, including the current user)
        var pagedResponse = new PagedResponse<LeaderboardProgressModel>(
            top10Users,
            request.PageNumber,
            request.PageSize,
            top10Users.Count
        );

        // Create the response object
        var response = new GetLeaderboardProgressForUserQueryResponse
        {
            CurrentPlayerUsername = currentUserProgress?.PlayerUsername,
            Data = pagedResponse,
            Succeeded = true,
            Message = "Leaderboard progress retrieved successfully"
        };

        return response;
    }
}