using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Commands.Create;

public record CreateLeaderboardTemplateCommand(CreateLeaderboardTemplateDto Create) : ICommand<LeaderboardTemplate>;
