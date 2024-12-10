using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule.Create;

public class CreateLeaderboardScheduleCommandHandler : ICommandHandler<CreateScheduleCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public CreateLeaderboardScheduleCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.CreateLeaderboardSchedule(request.Create);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
