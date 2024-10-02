using Wheel.Application.Models.Player;
using Wheel.Application.Models.Game;
using static Wheel.Application.Services.Concrete.GameService;

namespace Wheel.Application.Services.Abstract;

public interface IGameService
{
    InitialDataResponseModel GetInitialData();
    GameResponseModel GetGame();
    Task UpdateMetadataAsync();
    Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel command);
    Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel command);
    Task<(ConfigurationModel, List<RoundModel>)> CreateConfigurationAndRoundsAsync(
            string configurationName, int configurationValue, string rule,
            List<RoundModel> rounds, List<PriceModel> prices = null, List<SegmentModel> segments = null);
}