namespace GameLib.Application.Services.Abstract;

public interface IHubService
{
    Task BetTransactionAsync(int gameVersionId, int PromotionId, decimal amount);

    Task WinTransactionAsync(int gameVersionId, string coinId, int PromotionId, decimal amount);
}