using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule.Create;

public class CreateLeaderboardScheduleCommandHandler : ICommandHandler<CreateScheduleCommand, ApplicationResult<bool>>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public CreateLeaderboardScheduleCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult<bool>> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        return await _leaderBoardService.CreateLeaderboardSchedule(request.Create);
    }
}
