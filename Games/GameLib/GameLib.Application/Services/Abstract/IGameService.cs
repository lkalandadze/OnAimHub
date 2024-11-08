using GameLib.Application.Models.Game;

namespace GameLib.Application.Services.Abstract;

public interface IGameService
{
    InitialDataResponseModel GetInitialData();

    GameResponseModel GetGame();

    Task UpdateMetadataAsync();

    Task<GameShortInfoResponseModel> GetGameShortInfo();

    bool GameStatus();

    void ActivateGame();

    void DeactivateGame();
}