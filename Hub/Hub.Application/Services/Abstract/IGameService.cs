using Hub.Application.Models.Game;

namespace Hub.Application.Services.Abstract;
public interface IGameService
{
    IEnumerable<GameModel> GetGames();
    bool RemoveGame(string gameId);
    void AddOrUpdateGame(GameModel gameStatus);
}
