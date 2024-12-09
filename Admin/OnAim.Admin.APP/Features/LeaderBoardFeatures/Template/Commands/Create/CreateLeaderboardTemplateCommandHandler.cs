using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Commands.Create;

public sealed class CreateLeaderboardTemplateCommandHandler : ICommandHandler<CreateLeaderboardTemplateCommand, LeaderboardTemplate>
{
    private readonly ILeaderboardTemplateService _leaderboardTemplateService;

    public CreateLeaderboardTemplateCommandHandler(ILeaderboardTemplateService leaderboardTemplateService)
    {
        _leaderboardTemplateService = leaderboardTemplateService;
    }

    public async Task<LeaderboardTemplate> Handle(CreateLeaderboardTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _leaderboardTemplateService.CreateLeaderboardTemplate(request.Create);
    }
}
