﻿#nullable disable

using CheckmateValidations;
using GameLib.Domain.Abstractions;
using Wheel.Domain.Checkers;

namespace Wheel.Domain.Entities;

[CheckMate<WheelPrizeChecker>]
public class WheelPrize : BasePrize<Round>
{
    public WheelPrize()
    {
        
    }

    public WheelPrize(string name, int? wheelIndex = null, int? roundId = null) : base()
    {
        Name = name;
        WheelIndex = wheelIndex;
        RoundId = roundId;
    }

    public string Name { get; private set; }
    public int? WheelIndex { get; private set; }
    public int? RoundId { get; private set; }
}