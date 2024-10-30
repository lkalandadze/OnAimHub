using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule;

public class CreateScheduleCommandHandler : ICommandHandler<CreateScheduleCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public CreateScheduleCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.Schedule(request.TemplateId);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
