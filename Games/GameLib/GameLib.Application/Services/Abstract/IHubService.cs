namespace GameLib.Application.Services.Abstract;

public interface IHubService
{
    Task BetTransactionAsync(int gameVersionId, string CoinId, int amount);

    Task WinTransactionAsync(int gameVersionId, string CoinId, int amount);
}