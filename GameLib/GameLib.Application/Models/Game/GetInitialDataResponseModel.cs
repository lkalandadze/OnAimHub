#nullable disable

using GameLib.Domain.Abstractions;
using GameLib.Domain.Entities;

namespace GameLib.Application.Models.Game;

public class GetInitialDataResponseModel
{
    public Dictionary<string, List<BasePrizeGroup>> PrizeGroups { get; set; }
    public IEnumerable<Price> Prices { get; set; }
}