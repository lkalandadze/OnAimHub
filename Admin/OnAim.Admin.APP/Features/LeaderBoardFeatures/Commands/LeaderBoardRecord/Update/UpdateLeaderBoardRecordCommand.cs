using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.LeaderBoardRecord.Update;

public record UpdateLeaderBoardCommand(UpdateLeaderboardRecordCommand Update) : ICommand<ApplicationResult>;
