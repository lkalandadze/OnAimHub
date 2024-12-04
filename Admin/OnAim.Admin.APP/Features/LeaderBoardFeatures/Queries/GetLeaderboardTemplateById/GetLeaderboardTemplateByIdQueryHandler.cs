using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetLeaderboardTemplateById;

public class GetLeaderboardTemplateByIdQueryHandler : IQueryHandler<GetLeaderboardTemplateByIdQuery, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetLeaderboardTemplateByIdQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(GetLeaderboardTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        //var result = await _leaderBoardService.GetLeaderboardTemplateById(request.Id);

        return new ApplicationResult {Data = "" };
    }
}
