using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;

namespace Shared.Application.Generators;

internal abstract class Generator
{
    internal int Id { get; }
    internal List<Base.Prize> Prizes { get; }

    internal Generator(int id, List<Base.Prize> prizes)
    {
        Id = id;
        Prizes = prizes;
    }

    internal static Generator Create(IPrizeGroupRepository prizeGroupRepository, Base.PrizeGroup prizeGroup, PrizeGenerationType type)
    {
        return type == PrizeGenerationType.RNG
            ? new RNGPrizeGenerator(prizeGroup.Id, prizeGroup.Prizes.ToList())
            : new SequencePrizeGenerator(prizeGroupRepository, prizeGroup.Id, prizeGroup.Prizes.ToList(), prizeGroup.Sequence, prizeGroup.NextPrizeIndex!.Value);
    }

    internal abstract Base.Prize GetPrize();
}