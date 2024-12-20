#nullable disable

using GameLib.Application.Models.Price;
using GameLib.Domain.Abstractions;

namespace GameLib.Application.Models.Game;

public class InitialDataResponseModel
{
    public IEnumerable<dynamic> PrizeGroups { get; set; }
    public IEnumerable<PriceBaseGetModel> Prices { get; set; }
}