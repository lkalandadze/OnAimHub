namespace LevelService.Application.Services.Abstract;

public interface IPlayerService
{
    Task GrantExperienceAndRewardsAsync(int playerId, string currencyId, int amount);
}