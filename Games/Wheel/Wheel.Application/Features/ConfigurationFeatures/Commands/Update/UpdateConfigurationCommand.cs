using MediatR;

namespace Wheel.Application.Features.ConfigurationFeatures.Commands.Update;

public class UpdateConfigurationCommand : IRequest
{
    public string ConfigurationJson { get; set; }
}

//Test Data, use this, add prizetypes with ids 1 2 3 before testing

//{
//    "ConfigurationJson": "{ \"Name\": \"Hardcoded Wheel Configuration\", \"Value\": 1000, \"IsActive\": true, \"Rounds\": [ { \"Name\": \"Hardcoded Round 1\", \"Sequence\": [1, 2, 3], \"Prizes\": [ { \"Name\": \"Prize A\", \"Value\": 100, \"Probability\": 50, \"PrizeTypeId\": 1, \"WheelIndex\": 0 }, { \"Name\": \"Prize B\", \"Value\": 200, \"Probability\": 30, \"PrizeTypeId\": 2, \"WheelIndex\": 1 } ] }, { \"Name\": \"Hardcoded Round 2\", \"Sequence\": [1, 2, 3], \"Prizes\": [ { \"Name\": \"Prize C\", \"Value\": 150, \"Probability\": 40, \"PrizeTypeId\": 1, \"WheelIndex\": 2 }, { \"Name\": \"Prize D\", \"Value\": 250, \"Probability\": 20, \"PrizeTypeId\": 3, \"WheelIndex\": 3 } ] } ], \"Prices\": [ { \"Id\": \"Price5\", \"Value\": 200.50, \"Multiplier\": 1.5, \"CurrencyId\": \"OnAimCoin\" }, { \"Id\": \"Price6\", \"Value\": 350.75, \"Multiplier\": 2.0, \"CurrencyId\": \"OnAimCoin\" } ], \"Segments\": [ { \"Id\": \"Segment5\", \"IsDeleted\": true }, { \"Id\": \"Segment6\", \"IsDeleted\": false } ] }"
//}