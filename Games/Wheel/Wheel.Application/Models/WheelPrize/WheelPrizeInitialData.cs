#nullable disable

using Microsoft.AspNetCore.Identity;

namespace Wheel.Application.Models.WheelPrize;

public class WheelPrizeInitialData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public int? WheelIndex { get; set; }
    public string Coin { get; set; }
}