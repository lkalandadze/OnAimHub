using Leaderboard.Application.Features.LeaderboardResultFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.GetByPlayerId;

public class GetLeaderboardResultsByPlayerIdQueryHandler : IRequestHandler<GetLeaderboardResultsByPlayerIdQuery, GetLeaderboardResultsByPlayerIdQueryResponse>
{
    private readonly ILeaderboardResultRepository _leaderboardResultRepository;
    public GetLeaderboardResultsByPlayerIdQueryHandler(ILeaderboardResultRepository leaderboardResultRepository)
    {
        _leaderboardResultRepository = leaderboardResultRepository;
    }

    public async Task<GetLeaderboardResultsByPlayerIdQueryResponse> Handle(GetLeaderboardResultsByPlayerIdQuery request, CancellationToken cancellationToken)
    {
        var leaderboardResults = _leaderboardResultRepository.Query().Where(x => x.PlayerId == request.PlayerId);

        var total = leaderboardResults.Count();

        var leaderboardResultList = leaderboardResults.Pagination(request).ToList();

        var response = new GetLeaderboardResultsByPlayerIdQueryResponse
        {
            Data = new PagedResponse<LeaderboardResultModel>
            (
                leaderboardResultList?.Select(x => LeaderboardResultModel.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}