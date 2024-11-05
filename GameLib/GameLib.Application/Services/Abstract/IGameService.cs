using GameLib.Application.Models.Game;

namespace GameLib.Application.Services.Abstract;

public interface IGameService
{
    GetInitialDataResponseModel GetInitialData();

    Task<GetGameShortInfoModel> GetGameShortInfo();

    bool GameStatus();

    void ActivateGame();

    void DeactivateGame();
}