using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.CreateLeaderboardSchedule;

public class CreateLeaderboardScheduleCommandHandler : ICommandHandler<CreateLeaderboardScheduleCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public CreateLeaderboardScheduleCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(CreateLeaderboardScheduleCommand request, CancellationToken cancellationToken)
    {
        //var result = await _leaderBoardService.CreateLeaderboardSchedule(request.CreateLeaderboardSchedule);

        return new ApplicationResult {Data = "" };
    }
}
