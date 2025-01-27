using GameLib.Application.Models.Game;
using System.ComponentModel.DataAnnotations;
using Wheel.Application.Models.Wheel;

namespace Wheel.Application.Services.Abstract;

public interface IWheelService
{
    InitialDataResponseModel GetInitialData(int promotionId);

    Task<PlayResponseModel> PlayWheelAsync(int promotionId, int betPriceId);
}