using TestWheel.Application.Models.Player;

namespace TestWheel.Application.Services.Abstract;

public interface ITestWheelService
{
    Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel command);
    Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel command);
}