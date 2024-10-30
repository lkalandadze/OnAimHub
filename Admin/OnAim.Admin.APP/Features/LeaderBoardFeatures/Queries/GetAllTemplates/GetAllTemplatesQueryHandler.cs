using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllTemplates;

public class GetAllTemplatesQueryHandler : IQueryHandler<GetAllTemplatesQuery, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public GetAllTemplatesQueryHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.GetLeaderBoardTemplates(request.Filter);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
