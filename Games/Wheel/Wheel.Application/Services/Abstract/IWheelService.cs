using Wheel.Application.Models.Player;
using Wheel.Application.Models.Game;
using static Wheel.Application.Services.Concrete.WheelService;

namespace Wheel.Application.Services.Abstract;

public interface IWheelService
{
    GameResponseModel GetGame();
    Task UpdateMetadataAsync();
    Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel command);
    Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel command);
}