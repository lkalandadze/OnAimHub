using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Update;

public class UpdateLeaderBoardRecordCommandHandler : ICommandHandler<UpdateLeaderBoardCommand, ApplicationResult<bool>>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public UpdateLeaderBoardRecordCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult<bool>> Handle(UpdateLeaderBoardCommand request, CancellationToken cancellationToken)
    {
        return await _leaderBoardService.UpdateLeaderBoardRecord(request.Update);
    }
}
