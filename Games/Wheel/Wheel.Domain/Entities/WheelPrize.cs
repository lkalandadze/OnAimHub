#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using Wheel.Domain.Checkers;

namespace Wheel.Domain.Entities;

[CheckMate<WheelPrizeChecker>]
public class WheelPrize : BasePrize<WheelPrizeGroup>
{
    public int? WheelIndex { get; set; }
}