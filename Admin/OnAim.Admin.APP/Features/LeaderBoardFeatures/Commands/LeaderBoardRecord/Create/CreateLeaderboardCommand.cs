using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Create;

public record CreateLeaderboardCommand(CreateLeaderboardRecordCommand Create) : ICommand<ApplicationResult<bool>>;

