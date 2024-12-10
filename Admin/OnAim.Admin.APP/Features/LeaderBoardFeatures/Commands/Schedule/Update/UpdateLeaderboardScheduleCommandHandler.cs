using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule.Update;

public class UpdateLeaderboardScheduleCommandHandler : ICommandHandler<UpdateScheduleCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public UpdateLeaderboardScheduleCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.UpdateLeaderboardSchedule(request.Update);

        return new ApplicationResult
        {
            Data = result.Data,
            Success = result.Success,
        };
    }
}
