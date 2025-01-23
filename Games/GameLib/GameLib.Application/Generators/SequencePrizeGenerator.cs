using GameLib.Application.Managers;
using GameLib.Domain.Abstractions;

namespace GameLib.Application.Generators;

internal class SequencePrizeGenerator : Generator
{
    private object _sync = new();
    public int NextPrizeIndex { get; set; }
    public List<int> Sequence { get; }

    internal override PrizeGenerationType PrizeGenerationType => PrizeGenerationType.Sequence;

    public SequencePrizeGenerator(BasePrizeGroup prizeGroup, List<BasePrize> prizes, List<int> sequence, int nextPrizeIndex)
        : base(prizeGroup, prizes)
    {
        NextPrizeIndex = nextPrizeIndex;
        Sequence = sequence;
    }

    internal override BasePrize GeneratePrize()
    {
        lock (_sync)
        {
            BasePrize? prize;
            var prizeId = Sequence[NextPrizeIndex];
            prize = Prizes.First(p => p.Id == prizeId);

            NextPrizeIndex = (NextPrizeIndex + 1) % Sequence.Count;

            //TODO: save NextPrizeIndex in database (temporary)
            var prizeGroupRepository = RepositoryManager.PrizeGroupRepository(PrizeGroup.GetType());
            var existingPrizeGroup = prizeGroupRepository.OfIdAsync(PrizeGroup.Id).Result;
            existingPrizeGroup.NextPrizeIndex = NextPrizeIndex;
            prizeGroupRepository.Update(existingPrizeGroup);

            return prize;
        }
    }
}