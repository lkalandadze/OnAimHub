using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Repository;

namespace Shared.Application.Generators;

internal abstract class Generator
{
    internal int Id { get; }
    internal List<BasePrize> Prizes { get; }

    internal Generator(int id, List<BasePrize> prizes)
    {
        prizes = [];
        Id = id;
        Prizes = prizes;
    }

    internal static Generator Create(IBaseRepository<BasePrizeGroup> prizeGroupRepository, BasePrizeGroup prizeGroup, PrizeGenerationType type)
    {
        return type == PrizeGenerationType.RNG
            ? new RNGPrizeGenerator(prizeGroup.Id, prizeGroup.Prizes.ToList())
            : new SequencePrizeGenerator(prizeGroupRepository, prizeGroup.Id, prizeGroup.Prizes.ToList(), prizeGroup.Sequence, prizeGroup.NextPrizeIndex!.Value);
    }

    internal abstract BasePrize GetPrize();
}