using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Delete;

public record DeleteLeaderBoardRecordCommand(DeleteLeaderboardRecordCommand Delete) : ICommand<ApplicationResult>;
