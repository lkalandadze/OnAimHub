using Leaderboard.Application.Features.LeaderboardResultFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.GetPlayerResults;

public class GetPlayerResultsQueryHandler : IRequestHandler<GetPlayerResultsQuery, GetPlayerResultsQueryResponse>
{
    private readonly ILeaderboardResultRepository _leaderboardResultRepository;
    public GetPlayerResultsQueryHandler(ILeaderboardResultRepository leaderboardResultRepository)
    {
        _leaderboardResultRepository = leaderboardResultRepository;
    }

    public async Task<GetPlayerResultsQueryResponse> Handle(GetPlayerResultsQuery request, CancellationToken cancellationToken)
    {
        var leaderboardResults = _leaderboardResultRepository.Query().Where(x => x.PlayerId == request.PlayerId);

        var total = leaderboardResults.Count();

        var leaderboardResultList = leaderboardResults.Pagination(request).ToList();

        var response = new GetPlayerResultsQueryResponse
        {
            Data = new PagedResponse<PlayerResultModel>
            (
                leaderboardResultList?.Select(x => PlayerResultModel.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}