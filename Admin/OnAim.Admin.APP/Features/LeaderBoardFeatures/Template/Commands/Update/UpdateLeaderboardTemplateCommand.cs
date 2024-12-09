using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Commands.Update;

public record UpdateLeaderboardTemplateCommand(UpdateLeaderboardTemplateDto Update) : ICommand<ApplicationResult>;
