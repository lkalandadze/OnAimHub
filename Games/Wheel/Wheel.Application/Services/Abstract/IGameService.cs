using Shared.Application.Models.Consul;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;

namespace Wheel.Application.Services.Abstract;

public interface IGameService
{
    InitialDataResponseModel GetInitialData();
    List<GameRegisterResponseModel> GetGame();
    Task UpdateMetadataAsync();
    Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel command);
    Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel command);
}