using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Create;

public class CreateLeaderboardRecordCommandHandler : ICommandHandler<CreateLeaderboardRecordCommand, ApplicationResult>
{
    private readonly ILeaderBoardService _leaderBoardService;

    public CreateLeaderboardRecordCommandHandler(ILeaderBoardService leaderBoardService)
    {
        _leaderBoardService = leaderBoardService;
    }
    public async Task<ApplicationResult> Handle(CreateLeaderboardRecordCommand request, CancellationToken cancellationToken)
    {
        //var result = await _leaderBoardService.CreateLeaderBoardRecord(request.CreateLeaderboardRecordDto);

        return new ApplicationResult {Data = "" };
    }
}
