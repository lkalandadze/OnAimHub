using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Template.UpdateTemplate;

public record UpdateTemplateCommand(UpdateLeaderboardTemplateDto UpdateLeaderboardTemplateDto) : ICommand<ApplicationResult>;
