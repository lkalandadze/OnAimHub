using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Create;

public class CreateLeaderboardRecordCommandHandler : ICommandHandler<CreateLeaderboardCommand, ApplicationResult<bool>>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public CreateLeaderboardRecordCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult<bool>> Handle(CreateLeaderboardCommand request, CancellationToken cancellationToken)
    {
        return await _leaderBoardService.CreateLeaderBoardRecord(request.Create);
    }
}
