using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Template.UpdateTemplate;

public class UpdateTemplateCommandHandler : ICommandHandler<UpdateTemplateCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public UpdateTemplateCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        //var result = await _leaderBoardService.UpdateTemplate(request.UpdateLeaderboardTemplateDto);

        return new ApplicationResult { Data = "" };
    }
}
