using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Update;

public class UpdateLeaderBoardRecordCommandHandler : ICommandHandler<UpdateLeaderBoardRecordCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public UpdateLeaderBoardRecordCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(UpdateLeaderBoardRecordCommand request, CancellationToken cancellationToken)
    {
        //var result = await _leaderBoardService.UpdateLeaderBoardRecord(request.UpdateLeaderboardRecordDto);

        return new ApplicationResult {Data = "" };
    }
}
