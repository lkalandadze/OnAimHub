using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.UpdateLeaderboardSchedule;

public record UpdateLeaderboardScheduleCommand(UpdateLeaderboardScheduleDto UpdateLeaderboardSchedule) : ICommand<ApplicationResult>;
