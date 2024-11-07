using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.UpdateLeaderboardSchedule;

public class UpdateLeaderboardScheduleCommandHandler : ICommandHandler<UpdateLeaderboardScheduleCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public UpdateLeaderboardScheduleCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(UpdateLeaderboardScheduleCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.UpdateLeaderboardSchedule(request.UpdateLeaderboardSchedule);

        return new ApplicationResult
        {
            Data = result.Data,
            Success = result.Success,
        };
    }
}
