using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.UpdatePrizeType;

public class UpdatePrizeTypeCommandHandler : ICommandHandler<UpdatePrizeTypeCommand, ApplicationResult>
{
    private readonly IGameService _gameService;

    public UpdatePrizeTypeCommandHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(UpdatePrizeTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await _gameService.UpdatePrizeType(request.Id, request.CreatePrizeType);

        return new ApplicationResult { Data = result };
    }
}
