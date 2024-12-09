using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Create;

public record CreateCoinTemplateCommand(CreateCoinTemplateDto create) : ICommand<CoinTemplate>;
