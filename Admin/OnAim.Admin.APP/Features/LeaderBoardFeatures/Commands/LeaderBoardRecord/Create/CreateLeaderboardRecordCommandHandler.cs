using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Create;

public class CreateLeaderboardRecordCommandHandler : ICommandHandler<CreateLeaderboardCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public CreateLeaderboardRecordCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(CreateLeaderboardCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.CreateLeaderBoardRecord(request.Create);

        return new ApplicationResult {Data = result.Data, Success = result.Success };
    }
}
