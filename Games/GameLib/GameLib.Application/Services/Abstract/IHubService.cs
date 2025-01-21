namespace GameLib.Application.Services.Abstract;

public interface IHubService
{
    Task BetTransactionAsync(int gameVersionId, string sourceServiceName, int PromotionId, decimal amount);

    Task WinTransactionAsync(int gameVersionId, string sourceServiceName, string coinId, int PromotionId, decimal amount);
}