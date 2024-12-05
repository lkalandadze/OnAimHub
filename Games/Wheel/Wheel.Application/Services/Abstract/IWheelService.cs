using Wheel.Application.Models.Player;

namespace Wheel.Application.Services.Abstract;

public interface IWheelService
{
    Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel command);
}