using Hub.Application.Configurations;
using Hub.Application.Extensions;
using Hub.Application.Models.Balance;
using Hub.Application.Models.Player;
using Hub.Domain.Absractions.Repository;
using Microsoft.Extensions.Options;

namespace Hub.Application.Services;

public class BalanceService
{
    private readonly IPlayerRepository _player;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public BalanceService(IPlayerRepository player, HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration)
    {
        _player = player;
        _httpClient = httpClient;
        _casinoApiConfiguration = casinoApiConfiguration.Value;
    }

    public async Task<bool> ValidateBet(int playerId, int betAmout, string currencySymbol)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "id", playerId.ToString() }
        };

        var recievedBalance = await _httpClient.GetAsync<BalanceGetModel>(_casinoApiConfiguration.GetBalance, queryParams);

        if (recievedBalance == null)
        {
            throw new ArgumentNullException();
        }

        var balance = recievedBalance.Balances.First(x => x.Key == currencySymbol);

        return balance.Value > betAmout;
    }

    public async Task<double> FetchBalance(int playerId, string currencySymbol)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "id", playerId.ToString() }
        };

        var recievedBalance = await _httpClient.GetAsync<BalanceGetModel>(_casinoApiConfiguration.GetBalance, queryParams);

        if (recievedBalance == null)
        {
            throw new ArgumentNullException();
        }

        return recievedBalance.Balances.First(x => x.Key == currencySymbol).Value;
    }
}