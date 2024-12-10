using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule.Update;

public record UpdateScheduleCommand(UpdateLeaderboardScheduleCommand Update) : ICommand<ApplicationResult>;
