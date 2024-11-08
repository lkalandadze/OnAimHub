using Leaderboard.Application.Features.LeaderboardResultFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.Get;

public class GetLeaderboardResultQueryHandler : IRequestHandler<GetLeaderboardResultQuery, GetLeaderboardResultQueryResponse>
{
    private readonly ILeaderboardResultRepository _leaderboardResultRepository;
    public GetLeaderboardResultQueryHandler(ILeaderboardResultRepository leaderboardResultRepository)
    {
        _leaderboardResultRepository = leaderboardResultRepository;
    }

    public async Task<GetLeaderboardResultQueryResponse> Handle(GetLeaderboardResultQuery request, CancellationToken cancellationToken)
    {
        var leaderboardResults = _leaderboardResultRepository.Query().Where(x => x.LeaderboardRecordId == request.LeaderboardRecordId);

        var total = leaderboardResults.Count();

        var leaderboardResultList = leaderboardResults.Pagination(request).ToList();

        var response = new GetLeaderboardResultQueryResponse
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