using Shared.Domain.Abstractions;

namespace Shared.Application.Generators;

internal class RNGPrizeGenerator : Generator
{
    private int ProbabilitySum => Prizes.Sum(x => x.Probability);

    internal RNGPrizeGenerator(int id, List<BasePrize> prizes) 
        : base(id, prizes)
    {
    }

    internal override BasePrize GetPrize()
    {
        var randomNumber = RNG.RNG.Next(ProbabilitySum);
        var sum = 0;

        for (int i = 0; i < Prizes.Count; i++)
        {
            sum += Prizes[i].Probability;

            if(randomNumber < sum)
            {
                return Prizes[i];
            }
        }

        throw new ArgumentOutOfRangeException();
    }
}