using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Execute;

public record ExecuteCommand(int TemplateId) : ICommand<ApplicationResult>;
