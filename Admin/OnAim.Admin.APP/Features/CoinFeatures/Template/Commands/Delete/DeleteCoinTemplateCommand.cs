using OnAim.Admin.APP.CQRS.Command;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Commands.Delete;

public record DeleteCoinTemplateCommand(string Id) : ICommand<ApplicationResult<bool>>;
