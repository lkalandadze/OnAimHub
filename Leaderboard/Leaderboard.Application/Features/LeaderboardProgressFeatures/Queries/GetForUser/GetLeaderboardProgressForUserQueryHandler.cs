using Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;
using Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Get;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;
using System.Security.Claims;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.GetForUser;

public class GetLeaderboardProgressForUserQueryHandler : IRequestHandler<GetLeaderboardProgressForUserQuery, GetLeaderboardProgressForUserQueryResponse>
{
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetLeaderboardProgressForUserQueryHandler(ILeaderboardProgressRepository leaderboardProgressRepository, IHttpContextAccessor httpContextAccessor)
    {
        _leaderboardProgressRepository = leaderboardProgressRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetLeaderboardProgressForUserQueryResponse> Handle(GetLeaderboardProgressForUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }
        int currentUserId = int.Parse(userId);

        var top20Users = await _leaderboardProgressRepository.Query()
            .Where(x => x.LeaderboardRecordId == request.LeaderboardRecordId)
            .OrderByDescending(x => x.Amount)
            .Take(20)
            .Select(x => new LeaderboardProgressModel
            {
                PlayerId = x.PlayerId,
                PlayerUsername = x.PlayerUsername,
                Amount = x.Amount
            })
            .ToListAsync(cancellationToken);

        var currentUser = await _leaderboardProgressRepository.Query()
            .Where(x => x.LeaderboardRecordId == request.LeaderboardRecordId && x.PlayerId == currentUserId)
            .Select(x => new LeaderboardProgressModel
            {
                PlayerId = x.PlayerId,
                PlayerUsername = x.PlayerUsername,
                Amount = x.Amount
            })
            .FirstOrDefaultAsync(cancellationToken);

        var pagedResponse = new PagedResponse<LeaderboardProgressModel>(
            top20Users,
            request.PageNumber,
            request.PageSize,
            top20Users.Count
        );

        var response = new GetLeaderboardProgressForUserQueryResponse
        {
            Top20Users = top20Users,
            CurrentUser = currentUser,
            Data = pagedResponse,
            Succeeded = true,
            Message = "Leaderboard progress retrieved successfully"
        };

        return response;
    }
}