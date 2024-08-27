using Hub.Application.Models.Game;

namespace Hub.Application.Services.Abstract;
public interface IActiveGameService
{
    IEnumerable<ActiveGameModel> GetActiveGames();
    bool RemoveActiveGame(string gameId);
    void AddOrUpdateActiveGame(ActiveGameModel gameStatus);
}
