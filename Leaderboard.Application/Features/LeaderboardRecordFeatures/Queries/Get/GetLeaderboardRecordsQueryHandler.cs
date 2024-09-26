using Leaderboard.Application.Features.LeaderboardRecordFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.Get;

public class GetLeaderboardRecordsQueryHandler : IRequestHandler<GetLeaderboardRecordsQuery, GetLeaderboardRecordsQueryResponse>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;

    public GetLeaderboardRecordsQueryHandler(ILeaderboardRecordRepository leaderboardRecordRepository)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
    }

    public async Task<GetLeaderboardRecordsQueryResponse> Handle(GetLeaderboardRecordsQuery request, CancellationToken cancellationToken)
    {
        var leaderboardRecords = _leaderboardRecordRepository.Query();

        var total = leaderboardRecords.Count();

        var leaderboardRecordList = leaderboardRecords.Pagination(request).ToList();

        var response = new GetLeaderboardRecordsQueryResponse
        {
            Data = new PagedResponse<LeaderboardRecordsModel>
            (
                leaderboardRecordList?.Select(x => LeaderboardRecordsModel.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}