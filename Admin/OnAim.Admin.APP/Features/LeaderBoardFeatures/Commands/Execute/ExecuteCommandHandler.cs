using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Execute;

public class ExecuteCommandHandler : ICommandHandler<ExecuteCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public ExecuteCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(ExecuteCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.Execute(request.TemplateId);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
