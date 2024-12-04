using Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;
using System.Security.Claims;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Get;

public class GetLeaderboardProgressQueryHandler : IRequestHandler<GetLeaderboardProgressQuery, GetLeaderboardProgressQueryResponse>
{
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetLeaderboardProgressQueryHandler(ILeaderboardProgressRepository leaderboardProgressRepository, IHttpContextAccessor httpContextAccessor)
    {
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetLeaderboardProgressQueryResponse> Handle(GetLeaderboardProgressQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("PlayerId");

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        int currentUserId = int.Parse(userId);

        // Fetch all progress records for the given leaderboard
        var allProgressRecords = await _leaderboardProgressRepository.GetAllProgressAsync(request.LeaderboardRecordId, cancellationToken);

        // Sort all records by Amount in descending order
        var sortedRecords = allProgressRecords
            .OrderByDescending(x => x.Amount)
            .ToList();

        // Find the total number of records
        var total = sortedRecords.Count;

        // Get the top 10 users
        var top10Records = sortedRecords
            .Take(10)
            .Select(x => LeaderboardProgressModel.MapFrom(x))
            .ToList();

        // Find the current user's position and data
        var currentUserIndex = sortedRecords.FindIndex(x => x.PlayerId == currentUserId);

        LeaderboardProgressModel? currentUserRecord = null;

        if (currentUserIndex >= 0)
        {
            currentUserRecord = LeaderboardProgressModel.MapFrom(sortedRecords[currentUserIndex]);
        }

        // Combine the top 10 and the current user (if not in top 10)
        var responseRecords = top10Records;

        if (currentUserRecord != null && currentUserIndex >= 10)
        {
            responseRecords.Add(currentUserRecord);
        }

        // Create the response object
        var response = new GetLeaderboardProgressQueryResponse
        {
            Data = new PagedResponse<LeaderboardProgressModel>(
                responseRecords,
                request.PageNumber ?? 1,
                request.PageSize ?? 10,
                total
            ),
            Message = "Leaderboard progress retrieved successfully",
            Succeeded = true
        };

        return response;
    }
}