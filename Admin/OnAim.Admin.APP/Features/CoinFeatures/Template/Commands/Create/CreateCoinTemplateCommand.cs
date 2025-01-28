using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Create;

public record CreateCoinTemplateCommand(CreateCoinTemplateDto create) : ICommand<bool>;
