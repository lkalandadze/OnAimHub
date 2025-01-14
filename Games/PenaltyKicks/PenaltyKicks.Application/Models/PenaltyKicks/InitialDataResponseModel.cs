using GameLib.Application.Models.Price;

namespace PenaltyKicks.Application.Models.PenaltyKicks;

public class InitialDataResponseModel
{
    public bool HasActiveGame { get; set; }
    public GameConfigInitialDataResponseModel? GameConfigInfo { get; set; }
    public ActiveGameInfoInitialDataResponseModel? ActiveGameInfo { get; set; }

}

public class GameConfigInitialDataResponseModel
{
    public IEnumerable<PriceBaseGetModel>? BetPrices { get; set; }
    public int KicksCount { get; set; }
}

public class ActiveGameInfoInitialDataResponseModel
{
    public int CurrentKickIndex { get; set; }
    public int GoalsScored { get; set; }
    public int KicksRemaining { get; set; }
    public string BetPriceId { get; set; }
}