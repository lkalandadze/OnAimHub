using MediatR;
using Hub.Application.Extensions;
using Hub.Application.Models.Balance;
using Hub.Application.Configurations;
using Microsoft.Extensions.Options;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetBalance;

public class GetBalanceHandler : IRequestHandler<GetBalanceRequest, GetBalanceResponse>
{
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public GetBalanceHandler(HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration)
    {
        _httpClient = httpClient;
        _casinoApiConfiguration = casinoApiConfiguration.Value;
    }

    public async Task<GetBalanceResponse> Handle(GetBalanceRequest request, CancellationToken cancellationToken)
    {
        var endpoint = string.Format(_casinoApiConfiguration.Endpoints.GetBalance, request.PlayerId);

        var result = await _httpClient.GetAsync<BalanceGetModel>(_casinoApiConfiguration.Host, endpoint);

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