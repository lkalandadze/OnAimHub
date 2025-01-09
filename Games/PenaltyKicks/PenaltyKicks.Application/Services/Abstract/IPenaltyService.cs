using PenaltyKicks.Application.Models.PenaltyKicks;

namespace PenaltyKicks.Application.Services.Abstract;

public interface IPenaltyService
{
    Task<InitialDataResponseModel> GetInitialDataAsync(int promotionId);
}