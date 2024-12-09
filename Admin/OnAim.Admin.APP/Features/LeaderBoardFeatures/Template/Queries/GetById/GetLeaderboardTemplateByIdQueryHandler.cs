using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Queries.GetById;

public sealed class GetLeaderboardTemplateByIdQueryHandler : IQueryHandler<GetLeaderboardTemplateByIdQuery, ApplicationResult>
{
    private readonly ILeaderboardTemplateService _leaderboardTemplateService;

    public GetLeaderboardTemplateByIdQueryHandler(ILeaderboardTemplateService leaderboardTemplateService)
    {
        _leaderboardTemplateService = leaderboardTemplateService;
    }

    public async Task<ApplicationResult> Handle(GetLeaderboardTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _leaderboardTemplateService.GetLeaderboardTemplateById(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
