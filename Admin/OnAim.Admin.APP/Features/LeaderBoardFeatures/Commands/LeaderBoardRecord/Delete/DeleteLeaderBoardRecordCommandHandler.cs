using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Delete;

public sealed class DeleteLeaderBoardRecordCommandHandler : ICommandHandler<DeleteLeaderBoardRecordCommand, ApplicationResult<bool>>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public DeleteLeaderBoardRecordCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }

    public async Task<ApplicationResult<bool>> Handle(DeleteLeaderBoardRecordCommand request, CancellationToken cancellationToken)
    {
        return await _leaderBoardService.DeleteLeaderBoardRecord(request.Delete);
    }
}
