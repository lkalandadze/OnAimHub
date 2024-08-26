using Wheel.Application.Models;

namespace Wheel.Infrastructure.Services.Abstract;

public interface IGameService
{
    InitialDataResponseModel GetInitialData();
    GameVersionResponseModel GetGame();
    PlayResultModel Play(PlayRequestModel command);
}