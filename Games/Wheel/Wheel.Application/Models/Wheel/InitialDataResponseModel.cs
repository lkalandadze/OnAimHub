#nullable disable

using GameLib.Application.Models.Price;
using Wheel.Application.Models.WheelPrizeGroup;

namespace Wheel.Application.Models.Wheel;

public class InitialDataResponseModel
{
    public IEnumerable<WheelPrizeGroupInitialData> PrizeGroups { get; set; }
    public IEnumerable<PriceBaseGetModel> Prices { get; set; }
}