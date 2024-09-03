using Shared.Application.Models.Consul;

namespace Hub.Application.Services.Abstract;
public interface IActiveGameService
{
    IEnumerable<GameRegisterResponseModel> GetActiveGames();
    bool RemoveActiveGame(string gameId);
    void AddOrUpdateActiveGame(GameRegisterResponseModel gameStatus);
}
