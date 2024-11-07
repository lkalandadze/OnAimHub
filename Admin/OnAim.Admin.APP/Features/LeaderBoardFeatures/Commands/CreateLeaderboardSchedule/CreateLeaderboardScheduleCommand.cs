using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.CreateLeaderboardSchedule;

public record CreateLeaderboardScheduleCommand(CreateLeaderboardScheduleDto CreateLeaderboardSchedule) : ICommand<ApplicationResult>;
