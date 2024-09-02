using Wheel.Application.Models.Player;
using Wheel.Application.Models.Game;

namespace Wheel.Application.Services.Abstract;

public interface IGameService
{
    InitialDataResponseModel GetInitialData();
    GameVersionResponseModel GetGame();
    Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel command);
    Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel command);
}