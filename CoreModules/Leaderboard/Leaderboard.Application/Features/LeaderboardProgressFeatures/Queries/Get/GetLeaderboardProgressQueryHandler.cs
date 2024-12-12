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
        // Fetch all progress records for the given leaderboard
        var allProgressRecords = await _leaderboardProgressRepository.GetAllProgressAsync(request.LeaderboardRecordId, cancellationToken);

        // Sort all records by Amount in descending order and calculate placements
        var sortedRecords = allProgressRecords
            .OrderByDescending(x => x.Amount)
            .Select((x, index) => new LeaderboardProgressModel
            {
                LeaderboardRecordId = x.LeaderboardRecordId,
                PlayerId = x.PlayerId,
                PlayerUsername = x.PlayerUsername,
                Amount = x.Amount,
                Placement = index + 1 // Placement starts at 1
            })
            .ToList();

        // Find the total number of records
        var total = sortedRecords.Count;

        // Get the top 10 users
        var top10Records = sortedRecords.Take(10).ToList();

        // Create the response object
        var response = new GetLeaderboardProgressQueryResponse
        {
            Data = new PagedResponse<LeaderboardProgressModel>(
                top10Records,
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