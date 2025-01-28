using Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Admin.GetPlayerActiveLeaderboards;

public class GetPlayerActiveLeaderboardsQueryHandler : IRequestHandler<GetPlayerActiveLeaderboardsQuery, GetPlayerActiveLeaderboardsQueryResponse>
{
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly ILeaderboardRecordPrizeRepository _leaderboardRecordPrizeRepository;
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;

    public GetPlayerActiveLeaderboardsQueryHandler(
        ILeaderboardProgressRepository leaderboardProgressRepository,
        ILeaderboardRecordPrizeRepository leaderboardRecordPrizeRepository,
        ILeaderboardRecordRepository leaderboardRecordRepository)
    {
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _leaderboardRecordPrizeRepository = leaderboardRecordPrizeRepository;
        _leaderboardRecordRepository = leaderboardRecordRepository;
    }

    public async Task<GetPlayerActiveLeaderboardsQueryResponse> Handle(GetPlayerActiveLeaderboardsQuery request, CancellationToken cancellationToken)
    {
        // Fetch all active progress records for the given player ID
        var playerProgressRecords = await _leaderboardProgressRepository
            .GetUserAllActiveProgressesAsync(request.PlayerId, cancellationToken);

        if (!playerProgressRecords.Any())
        {
            return new GetPlayerActiveLeaderboardsQueryResponse
            {
                Data = new PagedResponse<UserLeaderboardProgressModel>(
                    Enumerable.Empty<UserLeaderboardProgressModel>(),
                    request.PageNumber ?? 1,
                    request.PageSize ?? 10,
                    0
                ),
                Message = "No active leaderboards found for the player.",
                Succeeded = true // Return true, no error occurred
            };
        }

        // Get all relevant leaderboard IDs
        var leaderboardIds = playerProgressRecords.Select(x => x.LeaderboardRecordId).Distinct();

        // Fetch leaderboard prizes for the relevant leaderboards
        var prizes = await _leaderboardRecordPrizeRepository
            .Query()
            .Where(x => leaderboardIds.Contains(x.LeaderboardRecordId))
            .ToListAsync(cancellationToken);

        // Fetch leaderboard records for the relevant leaderboards
        var leaderboardRecords = await _leaderboardRecordRepository
            .Query()
            .Where(x => leaderboardIds.Contains(x.Id)) // Assuming `Id` is the key in `LeaderboardRecord`
            .ToListAsync(cancellationToken);

        // Map the user's progress and prize details
        var userProgressRecords = new List<UserLeaderboardProgressModel>();

        foreach (var progress in playerProgressRecords)
        {
            // Get player's placement in the leaderboard
            var placement = await _leaderboardProgressRepository.GetPlacementAsync(progress.LeaderboardRecordId, progress.PlayerId, cancellationToken);

            // Find the corresponding prize for the player's placement
            var prize = prizes.FirstOrDefault(p => p.LeaderboardRecordId == progress.LeaderboardRecordId &&
                                                   placement >= p.StartRank && placement <= p.EndRank);

            // Find the leaderboard record for start/end date and promotion ID
            var leaderboardRecord = leaderboardRecords.FirstOrDefault(r => r.Id == progress.LeaderboardRecordId);

            // Map to the response model
            var userProgress = UserLeaderboardProgressModel.MapFrom(
                leaderboardProgress: progress,
                placement: placement,
                score: progress.Amount,
                coinId: prize?.CoinId,
                prizeAmount: prize?.Amount,
                promotionId: leaderboardRecord.PromotionId,
                startDate: leaderboardRecord?.StartDate ?? default,
                endDate: leaderboardRecord?.EndDate ?? default
            );

            userProgressRecords.Add(userProgress);
        }

        // Pagination logic
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? 10;
        var pagedRecords = userProgressRecords
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Create response
        return new GetPlayerActiveLeaderboardsQueryResponse
        {
            Data = new PagedResponse<UserLeaderboardProgressModel>(
                pagedRecords,
                pageNumber,
                pageSize,
                userProgressRecords.Count // Total number of records
            ),
            Message = "Player's active leaderboard progress retrieved successfully.",
            Succeeded = true
        };
    }
}