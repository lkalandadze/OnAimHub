using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Commands.Upsert;

public class UpsertProgressCommandHandler : IRequestHandler<UpsertProgressCommand>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpsertProgressCommandHandler(ILeaderboardRecordRepository leaderboardRecordRepository,
                                        ILeaderboardProgressRepository leaderboardProgressRepository,
                                        IHttpContextAccessor httpContextAccessor)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(UpsertProgressCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("PlayerId");
        var username = _httpContextAccessor.HttpContext?.User.FindFirstValue("UserName");

        if (userId == null || username == null)
            throw new Exception("User is not authenticated.");

        var leaderboard = await _leaderboardRecordRepository.Query().FirstOrDefaultAsync(x => x.Id == request.LeaderboardRecordId, cancellationToken);

        if (leaderboard == default || leaderboard.Status != LeaderboardRecordStatus.InProgress)
            throw new Exception("Leaderboard not found or is not in progress");

        int playerId = int.Parse(userId);

        var existingProgress = await _leaderboardProgressRepository
            .Query()
            .FirstOrDefaultAsync(x => x.LeaderboardRecordId == request.LeaderboardRecordId && x.PlayerId == playerId, cancellationToken);

        if (existingProgress != null)
        {
            existingProgress.Amount += request.GeneratedAmount;
            _leaderboardProgressRepository.Update(existingProgress);
            await _leaderboardProgressRepository.SaveChangesAsync(cancellationToken);
        }
        else
        {
            leaderboard.InsertProgress(playerId, username, request.GeneratedAmount);
            await _leaderboardRecordRepository.SaveChangesAsync(cancellationToken);
        }
    }
}