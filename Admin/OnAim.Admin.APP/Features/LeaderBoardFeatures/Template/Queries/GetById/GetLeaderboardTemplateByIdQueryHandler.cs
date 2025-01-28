using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Queries.GetById;

public sealed class GetLeaderboardTemplateByIdQueryHandler : IQueryHandler<GetLeaderboardTemplateByIdQuery, ApplicationResult<LeaderBoardTemplateListDto>>
{
    private readonly ILeaderboardTemplateService _leaderboardTemplateService;

    public GetLeaderboardTemplateByIdQueryHandler(ILeaderboardTemplateService leaderboardTemplateService)
    {
        _leaderboardTemplateService = leaderboardTemplateService;
    }

    public async Task<ApplicationResult<LeaderBoardTemplateListDto>> Handle(GetLeaderboardTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        return await _leaderboardTemplateService.GetLeaderboardTemplateById(request.Id);
    }
}
