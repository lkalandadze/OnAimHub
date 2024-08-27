using MediatR;
using Shared.Lib.Extensions;
using Hub.Application.Models.Balance;
using Hub.Application.Configurations;
using Microsoft.Extensions.Options;
using Hub.Application.Services.Abstract;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetBalance;

public class GetBalanceHandler : IRequestHandler<GetBalanceRequest, GetBalanceResponse>
{
    private readonly IAuthService _authService;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public GetBalanceHandler(IAuthService authService, HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration)
    {
        _authService = authService;
        _httpClient = httpClient;
        _casinoApiConfiguration = casinoApiConfiguration.Value;
    }

    public async Task<GetBalanceResponse> Handle(GetBalanceRequest request, CancellationToken cancellationToken)
    {
        var endpoint = string.Format(_casinoApiConfiguration.Endpoints.GetBalance, _authService.GetCurrentPlayerSegmentIds());

        var result = await _httpClient.CustomGetAsync<BalanceGetModel>(_casinoApiConfiguration.Host, endpoint);

        if (result == null || !result.Balances.Any())
        {
            throw new ArgumentNullException();
        }

        return new GetBalanceResponse
        {
            Balances = result.Balances,
        };
    }
}