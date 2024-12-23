using GameLib.Application.Models.Game;
using GameLib.Domain.Entities;

namespace GameLib.Application.Services.Abstract;

public interface IGameService
{
    GameResponseModel GetGame();

    Task UpdateMetadataAsync();

    Task<GameShortInfoResponseModel> GetGameShortInfo();

    bool GameStatus();

    void ActivateGame();

    void DeactivateGame();
}