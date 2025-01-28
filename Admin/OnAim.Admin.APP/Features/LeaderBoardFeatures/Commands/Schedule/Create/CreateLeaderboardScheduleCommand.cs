using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule.Create;

public record CreateScheduleCommand(CreateLeaderboardScheduleCommand Create) : ICommand<ApplicationResult<bool>>;
