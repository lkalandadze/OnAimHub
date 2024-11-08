namespace GameLib.Application.Services.Abstract;

public interface IHubService
{
    Task BetTransactionAsync(int gameVersionId, string currencyId, int amount);

    Task WinTransactionAsync(int gameVersionId, string currencyId, int amount);
}