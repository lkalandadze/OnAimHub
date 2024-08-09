using Hub.Domain.Absractions.Repository;

namespace Hub.Application.Services;

public class BalaceService
{
    private readonly IPlayerRepository _player;
    private readonly HttpClient _httpClient;

    public BalaceService(IPlayerRepository player, HttpClient httpClient)
    {
        _player = player;
        _httpClient = httpClient;
    }

    public async Task<bool> CheckPlayerBalance(int playerId, int betAmout)
    {
        return true;
    }
}