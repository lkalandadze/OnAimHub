using Hub.Application.Configurations;
using Hub.Application.Features.PlayerFeatures.Dtos;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayerBalance;

public class GetPlayerBalanceHandler : IRequestHandler<GetPlayerBalanceQuery, GetPlayerBalanceResponse>
{
    private readonly IAuthService _authService;
    private readonly IPlayerBalanceRepository _playerBalanceRepository;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public GetPlayerBalanceHandler(IAuthService authService, IPlayerBalanceRepository playerBalanceRepository, HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration)
    {
        _authService = authService;
        _playerBalanceRepository = playerBalanceRepository;
        _httpClient = httpClient;
        _casinoApiConfiguration = casinoApiConfiguration.Value;
    }

    public async Task<GetPlayerBalanceResponse> Handle(GetPlayerBalanceQuery request, CancellationToken cancellationToken)
    {
        var aaa = _authService.GetCurrentPlayer();

        if (_authService.GetCurrentPlayer() == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.UnauthorizedAccessAttempt, "Unauthorized access attempt - player information is missing.");
        }

        var balances = await _playerBalanceRepository.QueryAsync(x => x.PlayerId == _authService.GetCurrentPlayerId());

        return new GetPlayerBalanceResponse
        {
            Balances = balances.Select(x => PlayerBalanceBaseDtoModel.MapFrom(x)),
            PlayerId = _authService.GetCurrentPlayerId(),
        };
    }
}