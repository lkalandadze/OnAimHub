using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Delete;

public record DeleteGameConfigurationTemplateCommand(string Id) : ICommand<ApplicationResult>;
