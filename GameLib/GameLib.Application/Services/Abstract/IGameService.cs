using GameLib.Application.Models.Game;

namespace GameLib.Application.Services.Abstract;

public interface IGameService
{
    Task<GetGameShortInfoModel> GetGameShortInfo();

    bool GameStatus();

    void ActivateGame();

    void DeactivateGame();
}