using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllLeaderBoard;

public class GetAllLeaderBoardQueryHandler : IQueryHandler<GetAllLeaderBoardQuery, ApplicationResult<PaginatedResult<LeaderBoardListDto>>>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetAllLeaderBoardQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult<PaginatedResult<LeaderBoardListDto>>> Handle(GetAllLeaderBoardQuery request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.GetAllLeaderBoard(request.Filter);

        return new ApplicationResult<PaginatedResult<LeaderBoardListDto>> { Data = result.Data , Success = result.Success };
    }
}
