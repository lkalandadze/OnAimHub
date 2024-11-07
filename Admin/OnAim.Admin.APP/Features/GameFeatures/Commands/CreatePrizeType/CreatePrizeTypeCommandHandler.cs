using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.CreatePrizeType;

public class CreatePrizeTypeCommandHandler : ICommandHandler<CreatePrizeTypeCommand, ApplicationResult>
{
    private readonly IGameService _gameService;

    public CreatePrizeTypeCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(CreatePrizeTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await _gameService.CreatePrizeType(request.CreatePrizeType);

        return new ApplicationResult { Data = result };
    }
}
