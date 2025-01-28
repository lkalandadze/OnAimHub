using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Commands.Update;

public sealed class UpdateLeaderboardTemplateCommandHandler : ICommandHandler<UpdateLeaderboardTemplateCommand, ApplicationResult<bool>>
{
    private readonly ILeaderboardTemplateService _leaderboardTemplateService;

    public UpdateLeaderboardTemplateCommandHandler(ILeaderboardTemplateService leaderboardTemplateService)
    {
        _leaderboardTemplateService = leaderboardTemplateService;
    }

    public async Task<ApplicationResult<bool>> Handle(UpdateLeaderboardTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _leaderboardTemplateService.UpdateLeaderboardTemplate(request.Update);
    }
}
