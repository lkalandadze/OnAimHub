using Leaderboard.Application.Features.LeaderboardScheduleFeatures.DataModels;
using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Shared.Lib.Extensions;
using Shared.Lib.Wrappers;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.Queries.Get;

public class GetLeaderboardSchedulesQueryHandler : IRequestHandler<GetLeaderboardSchedulesQuery, GetLeaderboardSchedulesQueryResponse>
{
    private readonly ILeaderboardScheduleRepository _leaderboardScheduleRepository;
    public GetLeaderboardSchedulesQueryHandler(ILeaderboardScheduleRepository leaderboardScheduleRepository)
    {
        _leaderboardScheduleRepository = leaderboardScheduleRepository;
    }

    public async Task<GetLeaderboardSchedulesQueryResponse> Handle(GetLeaderboardSchedulesQuery request, CancellationToken cancellationToken)
    {
        var leaderboardSchedules = _leaderboardScheduleRepository.Query();

        var total = leaderboardSchedules.Count();

        var leaderboardTemplateList = leaderboardSchedules.Pagination(request).ToList();

        var response = new GetLeaderboardSchedulesQueryResponse
        {
            Data = new PagedResponse<LeaderboardSchedulesModel>
            (
                leaderboardTemplateList?.Select(x => LeaderboardSchedulesModel.MapFrom(x)),
                request.PageNumber,
                request.PageSize,
                total
            ),
        };

        return response;
    }
}