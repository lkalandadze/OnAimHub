using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Delete;

public record DeleteCoinTemplateCommand(string Id) : ICommand<ApplicationResult>;
