using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule.Update;

public class UpdateLeaderboardScheduleCommandHandler : ICommandHandler<UpdateScheduleCommand, ApplicationResult<bool>>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public UpdateLeaderboardScheduleCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult<bool>> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        return await _leaderBoardService.UpdateLeaderboardSchedule(request.Update);
    }
}
