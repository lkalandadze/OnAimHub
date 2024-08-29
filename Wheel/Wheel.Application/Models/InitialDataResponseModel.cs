#nullable disable

using GameLib.Domain.Abstractions;
using GameLib.Domain.Entities;

namespace Wheel.Application.Models;

public class InitialDataResponseModel
{
    public Dictionary<string, List<BasePrizeGroup>> PrizeGroups { get; set; }
    public IEnumerable<Price> Prices { get; set; }
}