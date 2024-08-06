using Shared.Domain.Abstractions;

namespace Shared.Application.Generators;

internal class SequencePrizeGenerator : Generator
{
    private object _sync = new();
    public int NextPrizeIndex { get; set; }
    public List<int> Sequence { get; }

    internal override PrizeGenerationType PrizeGenerationType => PrizeGenerationType.Sequence;

    public SequencePrizeGenerator(int id, List<BasePrize> prizes, List<int> sequence, int nextPrizeIndex)
        : base(id, prizes)
    {
        NextPrizeIndex = nextPrizeIndex;
        Sequence = sequence;
    }

    internal override BasePrize GetPrize()
    {
        lock (_sync)
        {
            var currentPrizeIndex = NextPrizeIndex;
            NextPrizeIndex++;

            if(NextPrizeIndex >= Sequence.Count)
            {
                NextPrizeIndex = 0;
            }

            return Prizes[currentPrizeIndex];
        }
    }
}