using Shared.Domain.Entities;

namespace Shared.Application.Generators;

internal class RNGPrizeGenerator : Generator
{
    private int ProbabilitySum => Prizes.Sum(x => x.Probability);

    public RNGPrizeGenerator(int id, List<Base.Prize> prizes) 
        : base(id, prizes)
    {
    }

    internal override Base.Prize GetPrize()
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