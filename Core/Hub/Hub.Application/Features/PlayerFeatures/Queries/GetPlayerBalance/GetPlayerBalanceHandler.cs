using Hub.Application.Features.PlayerFeatures.Dtos;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayerBalance;

public class GetPlayerBalanceHandler : IRequestHandler<GetPlayerBalanceQuery, GetPlayerBalanceResponse>
{
    private readonly IAuthService _authService;
    private readonly IPlayerBalanceRepository _playerBalanceRepository;

    public GetPlayerBalanceHandler(IAuthService authService, IPlayerBalanceRepository playerBalanceRepository)
    {
        _authService = authService;
        _playerBalanceRepository = playerBalanceRepository;
    }

    public async Task<GetPlayerBalanceResponse> Handle(GetPlayerBalanceQuery request, CancellationToken cancellationToken)
    {
        var player = _authService.GetCurrentPlayer();

        if (player == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.UnauthorizedAccessAttempt, "Unauthorized access attempt - player information is missing.");
        }

        var balances = _playerBalanceRepository.Query(x => x.PlayerId == player.Id);

        if (request.PromotionId != null)
        {
            balances = balances.Where(b => b.PromotionId == request.PromotionId.Value);
        }

        return new GetPlayerBalanceResponse
        {
            Balances = (await balances.ToListAsync()).Select(x => PlayerBalanceBaseDtoModel.MapFrom(x)).OrderBy(b => b.Id),
            PlayerId = player.Id,
        };
    }
}