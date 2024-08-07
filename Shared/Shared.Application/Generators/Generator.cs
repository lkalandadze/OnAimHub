﻿using Shared.Domain.Abstractions;

namespace Shared.Application.Generators;

internal abstract class Generator
{
    internal int Id { get; }
    internal List<BasePrize> Prizes { get; }
    abstract internal PrizeGenerationType PrizeGenerationType { get; }


    internal Generator(int id, List<BasePrize> prizes)
    {
        prizes = [];
        Id = id;
        Prizes = prizes;
    }

    internal static Generator Create(BasePrizeGroup prizeGroup, PrizeGenerationType type)
    {
        return type == PrizeGenerationType.RNG
            ? new RNGPrizeGenerator(prizeGroup.Id, prizeGroup.Prizes.ToList())
            : new SequencePrizeGenerator(prizeGroup.Id, prizeGroup.Prizes.ToList(), prizeGroup.Sequence, prizeGroup.NextPrizeIndex!.Value);
    }

    internal abstract BasePrize GetPrize();
}