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
    private readonly ILeaderboardRecordPrizeRepository _leaderboardRecordPrizeRepository;

    public GetLeaderboardProgressQueryHandler(ILeaderboardProgressRepository leaderboardProgressRepository, IHttpContextAccessor httpContextAccessor, ILeaderboardRecordPrizeRepository leaderboardRecordPrizeRepository)
    {
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _httpContextAccessor = httpContextAccessor;
        _leaderboardRecordPrizeRepository = leaderboardRecordPrizeRepository;
    }

    public async Task<GetLeaderboardProgressQueryResponse> Handle(GetLeaderboardProgressQuery request, CancellationToken cancellationToken)
    {
        // Fetch all progress records for the given leaderboard
        var allProgressRecords = await _leaderboardProgressRepository.GetAllProgressAsync(request.LeaderboardRecordId, cancellationToken);

        var prizes = _leaderboardRecordPrizeRepository.Query().Where(x => x.LeaderboardRecordId == request.LeaderboardRecordId);

        // Sort all records by Amount in descending order and calculate placements
        var sortedRecords = allProgressRecords
            .OrderByDescending(x => x.Amount)
            .Select((x, index) =>
            {
                var placement = index + 1; // Placement starts at 1

                // Find the prize for the current placement
                var prize = prizes.FirstOrDefault(p => placement >= p.StartRank && placement <= p.EndRank);

                return new LeaderboardProgressModel
                {
                    LeaderboardRecordId = x.LeaderboardRecordId,
                    PlayerId = x.PlayerId,
                    PlayerUsername = x.PlayerUsername,
                    Amount = x.Amount,
                    Placement = placement,
                    CoinId = prize?.CoinId, // Assign CoinId if a prize exists
                    PrizeAmount = prize?.Amount // Assign Prize Amount if a prize exists
                };
            })
            .ToList();

        // Find the total number of records
        var total = sortedRecords.Count;

        // Create the response object
        var response = new GetLeaderboardProgressQueryResponse
        {
            Data = new PagedResponse<LeaderboardProgressModel>(
                sortedRecords,
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