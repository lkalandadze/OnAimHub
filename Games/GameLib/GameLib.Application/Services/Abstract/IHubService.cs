namespace GameLib.Application.Services.Abstract;

public interface IHubService
{
    Task BetTransactionAsync(int gameVersionId, int PromotionId, int amount);

    Task WinTransactionAsync(int gameVersionId, int PromotionId, int amount);
}