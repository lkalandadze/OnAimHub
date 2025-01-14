using GameLib.Domain.Abstractions;
using PenaltyKicks.Application.Models.PenaltyKicks;

namespace PenaltyKicks.Application.Services.Abstract;

public interface IPenaltyService
{
    InitialDataResponseModel GetInitialDataAsync(int promotionId);

    Task<BetResponseModel> PlaceBetAsync(int promotionId, string betPriceId);

    Task<KickResponseModel> PenaltyKick(int promotionId);
}