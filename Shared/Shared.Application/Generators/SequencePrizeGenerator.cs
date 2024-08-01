using Shared.Domain.Entities;

namespace Shared.Application.Generators;

internal class SequencePrizeGenerator : Generator
{
    private object _sync = new();

    public int NextPrizeIndex { get; set; }
    public List<int> Sequence { get; } 

    public SequencePrizeGenerator(int id, List<Prize> prizes, List<int> sequence, int nextPrizeIndex) 
        : base(id, prizes)
    {
        NextPrizeIndex = nextPrizeIndex;
        Sequence = sequence;
    }

    internal override Prize GetPrize()
    {
        lock (_sync)
        {
            var currentPrizeIndex = NextPrizeIndex;
            NextPrizeIndex = NextPrizeIndex + 1;
            if(NextPrizeIndex >= Sequence.Count)
            {
                NextPrizeIndex = 0;
            }

            SaveNextPrizeIndexInDb(NextPrizeIndex);

            return Prizes[currentPrizeIndex];
        }
    }

    private void SaveNextPrizeIndexInDb(int nextPrizeIndex)
    {
        
    }
}