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

    internal BasePrize GetPrize(int playerId)
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

            if (prize.RemainingGlobalSetLimit <= 0)
            {
                continue;
            }

            var limitedPlayerPrizeRepository = RepositoryManager.LimitedPrizeCountsByPlayerRepository();

            var limitedPlayerPrize = limitedPlayerPrizeRepository
                .Query(pp => pp.PrizeId == prize.Id && pp.PlayerId == playerId)
                .FirstOrDefault();

            if (!(limitedPlayerPrize?.Count >= prize.PerPlayerSetLimit))
            {
                prize.DecrementRemainingGlobalLimit();
                
                if (limitedPlayerPrize != null)
                {
                    limitedPlayerPrize.IncreaseCount();
                    limitedPlayerPrizeRepository.Update(limitedPlayerPrize);
                }
                else
                {
                    var limitedPrizeCount = new LimitedPrizeCountsByPlayer(playerId, prize.Id);
                    limitedPrizeCount.IncreaseCount();
                    limitedPlayerPrizeRepository.InsertAsync(limitedPrizeCount);
                }

                var totalSetWinCount = limitedPlayerPrizeRepository
                    .Query(pp => pp.PrizeId == prize.Id && pp.Count >= prize.SetSize)
                    .Count();

                if ((prize.GlobalSetLimit - totalSetWinCount) > prize.RemainingGlobalSetLimit)
                {
                    prize.RemainingGlobalSetLimit = prize.GlobalSetLimit - totalSetWinCount;
                }

                //TODO: save remaining ...
                //RepositoryManager.PrizeGroupRepository(Prizes.First().GetType()).Update(PrizeGroup);
                limitedPlayerPrizeRepository.SaveAsync();

                break;
            }
        }

        return prize;
    }

    internal abstract BasePrize GeneratePrize();
}