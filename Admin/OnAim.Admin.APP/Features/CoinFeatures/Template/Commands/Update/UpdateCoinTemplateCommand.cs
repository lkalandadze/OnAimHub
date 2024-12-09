using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Update;

public record UpdateCoinTemplateCommand(UpdateCoinTemplateDto update) : ICommand<ApplicationResult>;
