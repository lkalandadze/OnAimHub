#nullable disable

using GameLib.Application.Models.Price;
using Wheel.Application.Models.Round;

namespace Wheel.Application.Models.Wheel;

public class InitialDataResponseModel
{
    public IEnumerable<RoundInitialData> Rounds { get; set; }
    public IEnumerable<PriceBaseGetModel> Prices { get; set; }
}