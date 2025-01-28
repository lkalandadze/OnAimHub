using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Queries.GetAll;

public sealed class GetAllLeaderboardTemplatesQueryHandler : IQueryHandler<GetAllLeaderboardTemplatesQuery, ApplicationResult<PaginatedResult<LeaderBoardTemplateListDto>>>
{
    private readonly ILeaderboardTemplateService _leaderboardTemplateService;

    public GetAllLeaderboardTemplatesQueryHandler(ILeaderboardTemplateService leaderboardTemplateService)
    {
        _leaderboardTemplateService = leaderboardTemplateService;
    }

    public async Task<ApplicationResult<PaginatedResult<LeaderBoardTemplateListDto>>> Handle(GetAllLeaderboardTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _leaderboardTemplateService.GetAllLeaderboardTemplates(request.Filter);
    }
}
