using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Create;

public record CreateLeaderboardRecordCommand(CreateLeaderboardRecordDto CreateLeaderboardRecordDto) : ICommand<ApplicationResult>;

