using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Template.CreateTemplate;

public record CreateTemplateCommand(CreateLeaderboardTemplateDto CreateLeaderboardTemplateDto) : ICommand<ApplicationResult>;
