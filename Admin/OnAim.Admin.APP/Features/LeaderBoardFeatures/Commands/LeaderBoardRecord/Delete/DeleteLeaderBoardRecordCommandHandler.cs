using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Delete;

public sealed class DeleteLeaderBoardRecordCommandHandler : ICommandHandler<DeleteLeaderBoardRecordCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public DeleteLeaderBoardRecordCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }

    public async Task<ApplicationResult> Handle(DeleteLeaderBoardRecordCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaderBoardService.DeleteLeaderBoardRecord(request.Delete);

        return new ApplicationResult { Data = result.Data, Success = result.Success };
    }
}
