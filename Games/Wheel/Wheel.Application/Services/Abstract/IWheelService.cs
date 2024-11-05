using Wheel.Application.Models.Player;

namespace Wheel.Application.Services.Abstract;

public interface IWheelService
{
    Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel command);
    Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel command);
}