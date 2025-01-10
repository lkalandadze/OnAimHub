#nullable disable

using GameLib.Application.Models.Price;

namespace PenaltyKicks.Application.Models.PenaltyKicks;

public class InitialDataResponseModel
{
    public IEnumerable<PriceBaseGetModel> Prices { get; set; }
    public int KicksCount { get; set; }
}