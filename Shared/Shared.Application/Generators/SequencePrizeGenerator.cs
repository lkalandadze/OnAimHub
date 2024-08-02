using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;

namespace Shared.Application.Generators;

internal class SequencePrizeGenerator : Generator
{
    private readonly IPrizeGroupRepository _prizeGroupRepository;
    private object _sync = new();

    public int NextPrizeIndex { get; set; }
    public List<int> Sequence { get; } 

    public SequencePrizeGenerator(IPrizeGroupRepository prizeGroupRepository, int id, List<Base.Prize> prizes, List<int> sequence, int nextPrizeIndex) : base(id, prizes)
    {
        NextPrizeIndex = nextPrizeIndex;
        Sequence = sequence;
        _prizeGroupRepository = prizeGroupRepository;
    }

    internal override Base.Prize GetPrize()
    {
        lock (_sync)
        {
            var currentPrizeIndex = NextPrizeIndex;
            NextPrizeIndex++;

            if(NextPrizeIndex >= Sequence.Count)
            {
                NextPrizeIndex = 0;
            }

            SaveNextPrizeIndexInDb(NextPrizeIndex);

            return Prizes[currentPrizeIndex];
        }
    }

    private async void SaveNextPrizeIndexInDb(int nextPrizeIndex)
    {
        var prizeGroup = await _prizeGroupRepository.OfIdAsync(Id);

        if (prizeGroup != null)
        {
            prizeGroup.NextPrizeIndex = nextPrizeIndex;
            await _prizeGroupRepository.SaveAsync();
        }
    }
}