using GameLib.Application.Models.Game;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;

namespace Wheel.Application.Services.Abstract;

public interface IWheelService
{
    InitialDataResponseModel GetInitialData(int promotionId);

    Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel command);
}