using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Template.Commands.Delete;

public record DeleteLeaderboardTemplateCommand(string Id) : ICommand<ApplicationResult<bool>>;
