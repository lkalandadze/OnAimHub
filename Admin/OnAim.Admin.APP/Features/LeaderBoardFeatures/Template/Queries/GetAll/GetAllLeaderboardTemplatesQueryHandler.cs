using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Queries.GetAll;

public sealed class GetAllLeaderboardTemplatesQueryHandler : IQueryHandler<GetAllLeaderboardTemplatesQuery, ApplicationResult>
{
    private readonly ILeaderboardTemplateService _leaderboardTemplateService;

    public GetAllLeaderboardTemplatesQueryHandler(ILeaderboardTemplateService leaderboardTemplateService)
    {
        _leaderboardTemplateService = leaderboardTemplateService;
    }

    public async Task<ApplicationResult> Handle(GetAllLeaderboardTemplatesQuery request, CancellationToken cancellationToken)
    {
        var result = await _leaderboardTemplateService.GetAllLeaderboardTemplates(request.Filter);

        return new ApplicationResult { Data = result.Data ,  Success = result.Success };
    }
}
