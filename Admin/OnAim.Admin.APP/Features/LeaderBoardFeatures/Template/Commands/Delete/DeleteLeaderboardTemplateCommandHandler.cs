using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Commands.Delete;

public sealed class DeleteLeaderboardTemplateCommandHandler : ICommandHandler<DeleteLeaderboardTemplateCommand, ApplicationResult>
{
    private readonly ILeaderboardTemplateService _leaderboardTemplateService;

    public DeleteLeaderboardTemplateCommandHandler(ILeaderboardTemplateService leaderboardTemplateService)
    {
        _leaderboardTemplateService = leaderboardTemplateService;
    }

    public async Task<ApplicationResult> Handle(DeleteLeaderboardTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaderboardTemplateService.DeleteLeaderboardTemplate(request.Id);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
