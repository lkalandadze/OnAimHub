using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Template.CreateTemplate;

public class CreateTemplateCommandHandler : ICommandHandler<CreateTemplateCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public CreateTemplateCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.CreateTemplate(request.CreateLeaderboardTemplateDto);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
