using GameLib.Application.Managers;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Entities;

namespace GameLib.Application.Generators;

internal abstract class Generator
{
    internal BasePrizeGroup PrizeGroup { get; }
    internal List<BasePrize> Prizes = [];

    abstract internal PrizeGenerationType PrizeGenerationType { get; }

    internal Generator(BasePrizeGroup prizeGroup, List<BasePrize> prizes)
    {
        PrizeGroup = prizeGroup;
        Prizes = prizes;
    }

    internal static Generator Create(BasePrizeGroup prizeGroup, PrizeGenerationType generationType)
    {
        return generationType == PrizeGenerationType.RNG
            ? new RNGPrizeGenerator(prizeGroup, prizeGroup.GetBasePrizes())
            : new SequencePrizeGenerator(prizeGroup, prizeGroup.GetBasePrizes(), prizeGroup.Sequence, prizeGroup.NextPrizeIndex!.Value);
    }

    internal BasePrize GetPrize(int? playerId = null)
    {
        BasePrize? prize = null;
        int tryCounts = 0;

        while (true)
        {
            if (tryCounts++ > 10)
            {
                throw new Exception("Too much try times");
            }

            prize = GeneratePrize();

            if (prize == null)
            {
                continue;
            }

            if (prize.RemainingGlobalLimit <= 0)
            {
                continue;
            }

            var limitedPlayerPrize = RepositoryManager.LimitedPrizeCountsByPlayerRepository()
                .Query(pp => pp.PrizeId == prize.Id && pp.PlayerId == playerId)
                .FirstOrDefault();

            if (!(limitedPlayerPrize?.Count >= prize.PerPlayerLimit))
            {
                prize.DecrementRemainingGlobalLimit();
                var repository = RepositoryManager.LimitedPrizeCountsByPlayerRepository();

                if (limitedPlayerPrize != null)
                {
                    limitedPlayerPrize.IncreaseCount();
                    repository.Update(limitedPlayerPrize);
                    
                }
                else
                {
                    var limitedPrizeCount = new LimitedPrizeCountsByPlayer(playerId!.Value, prize.Id);
                    limitedPrizeCount.IncreaseCount();
                    repository.InsertAsync(limitedPrizeCount);
                }

                repository.SaveAsync();

                // save prize RemainingGlobalLimit

                break;
            }
        }

        return prize;
    }

    internal abstract BasePrize GeneratePrize();
}