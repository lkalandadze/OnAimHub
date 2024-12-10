using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Update;

public class UpdateLeaderBoardRecordCommandHandler : ICommandHandler<UpdateLeaderBoardCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public UpdateLeaderBoardRecordCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(UpdateLeaderBoardCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.UpdateLeaderBoardRecord(request.Update);

        return new ApplicationResult {Data = result.Data, Success = result.Success };
    }
}
