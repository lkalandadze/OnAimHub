#nullable disable

using Shared.Domain.Abstractions;
using Shared.Domain.Entities;

namespace Wheel.Application.Models;

public class InitialDataResponseModel
{
    public Dictionary<string, List<BasePrizeGroup>> PrizeGroups { get; set; }
    public IEnumerable<Price> Prices { get; set; }
}