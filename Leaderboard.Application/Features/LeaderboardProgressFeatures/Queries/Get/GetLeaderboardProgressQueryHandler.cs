using Leaderboard.Application.Features.LeaderboardProgressFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Get;

public class GetLeaderboardProgressQueryHandler : IRequestHandler<GetLeaderboardProgressQuery, GetLeaderboardProgressQueryResponse>
{
    private readonly ILeaderboardProgressRepository _leaderboardProgressRepository;
    public GetLeaderboardProgressQueryHandler(ILeaderboardProgressRepository leaderboardProgressRepository)
    {
        _leaderboardProgressRepository = leaderboardProgressRepository;
    }

    public async Task<GetLeaderboardProgressQueryResponse> Handle(GetLeaderboardProgressQuery request, CancellationToken cancellationToken)
    {
        var leaderboardRecords = _leaderboardProgressRepository.Query();

        var total = leaderboardRecords.Count();

        var leaderboardRecordList = leaderboardRecords.Pagination(request).ToList();

        var response = new GetLeaderboardProgressQueryResponse
        {
            Data = new PagedResponse<LeaderboardProgressModel>
            (
                leaderboardRecordList?.Select(x => LeaderboardProgressModel.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}