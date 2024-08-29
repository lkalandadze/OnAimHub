using Wheel.Application.Models;
using Wheel.Application.Models.Player;

namespace Wheel.Infrastructure.Services.Abstract;

public interface IGameService
{
    InitialDataResponseModel GetInitialData();
    GameVersionResponseModel GetGame();
    Task<PlayResultModel> PlayJackpotAsync(PlayRequestModel command);
    Task<PlayResultModel> PlayWheelAsync(PlayRequestModel command);
}