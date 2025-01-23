using GameLib.Domain.Abstractions;

namespace GameLib.Application.Generators;

internal class RNGPrizeGenerator : Generator
{
    private int ProbabilitySum => Prizes.Sum(x => x.Probability);

    internal override PrizeGenerationType PrizeGenerationType => PrizeGenerationType.RNG;

    internal RNGPrizeGenerator(BasePrizeGroup prizeGroup, List<BasePrize> prizes)
        : base(prizeGroup, prizes)
    {
    }

    internal override BasePrize GeneratePrize()
    {
        var randomNumber = RNG.RNG.Next(ProbabilitySum);
        var sum = 0;

        foreach (var prize in Prizes)
        {
            sum += prize.Probability;

            if (randomNumber < sum)
            {
                return prize;
            }
        }

        throw new ArgumentOutOfRangeException();
    }
}