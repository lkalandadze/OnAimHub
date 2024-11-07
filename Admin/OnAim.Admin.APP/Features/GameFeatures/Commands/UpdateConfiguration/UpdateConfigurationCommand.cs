using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.UpdateConfiguration;

public record UpdateConfigurationCommand(string ConfigurationJson) : ICommand<ApplicationResult>;
