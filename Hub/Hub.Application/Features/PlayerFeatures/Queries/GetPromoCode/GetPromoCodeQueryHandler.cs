using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPromoCode;

public class GetPromoCodeQueryHandler : IRequestHandler<GetPromoCodeQuery, string>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IAuthService _authService;
    public GetPromoCodeQueryHandler(IPlayerRepository playerRepository, IAuthService authService)
    {
        _playerRepository = playerRepository;
        _authService = authService;
    }

    public async Task<string> Handle(GetPromoCodeQuery request, CancellationToken cancellationToken)
    {
        int playerId = _authService.GetCurrentPlayerId();

        var promoCode = Player.IdToPromo(playerId);

        return promoCode;
    }
}