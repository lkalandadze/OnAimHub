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
            // Deserialize cached data for top 10
            top10Users = JsonSerializer.Deserialize<List<LeaderboardProgressModel>>(cachedData);
        }
        else
        {
            // Fetch all leaderboard progress from the repository
            var allProgress = await _leaderboardProgressRepository.GetAllProgressAsync(request.LeaderboardRecordId, cancellationToken);

            // Sort by amount (score) in descending order and map to LeaderboardProgressModel
            var allProgressData = allProgress
                .OrderByDescending(x => x.Amount)
                .Select(x => new LeaderboardProgressModel
                {
                    LeaderboardRecordId = x.LeaderboardRecordId,
                    PlayerId = x.PlayerId,
                    PlayerUsername = x.PlayerUsername,
                    Amount = x.Amount
                })
                .ToList();

            // Get top 10 users
            top10Users = allProgressData.Take(10).ToList();

            // Cache the result with a TTL of 1 minute
            var serializedData = JsonSerializer.Serialize(top10Users);
            await _cache.StringSetAsync(cacheKey, serializedData, TimeSpan.FromMinutes(1));
        }

        // Fetch all progress to calculate placements
        var allProgressList = await _leaderboardProgressRepository.GetAllProgressAsync(request.LeaderboardRecordId, cancellationToken);

        // Calculate placements for all users
        var sortedProgress = allProgressList
            .OrderByDescending(x => x.Amount)
            .Select((x, index) => new LeaderboardProgressModel
            {
                LeaderboardRecordId = x.LeaderboardRecordId,
                PlayerId = x.PlayerId,
                PlayerUsername = x.PlayerUsername,
                Amount = x.Amount,
                Placement = index + 1 // Placement starts from 1
            })
            .ToList();

        // Find current user's progress and placement
        var currentUserProgress = sortedProgress.FirstOrDefault(x => x.PlayerId == currentUserId);

        if (currentUserProgress == null)
            throw new InvalidOperationException("Current user's progress not found in the leaderboard.");

        // Paginate the top 10
        var pagedResponse = new PagedResponse<LeaderboardProgressModel>(
            sortedProgress.Take(10).ToList(), // Return top 10 for the response
            request.PageNumber,
            request.PageSize,
            sortedProgress.Count
        );

        // Create the response object
        var response = new GetLeaderboardProgressForUserQueryResponse
        {
            CurrentPlayerUsername = currentUserProgress.PlayerUsername,
            CurrentPlacement = currentUserProgress.Placement,
            Data = pagedResponse,
            Succeeded = true,
            Message = "Leaderboard progress retrieved successfully"
        };

        return response;
    }

}