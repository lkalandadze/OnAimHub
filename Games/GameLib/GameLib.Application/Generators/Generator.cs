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

        while (!IsPrizeValid(prize = GeneratePrize(), playerId))
        {
            if (tryCounts++ > 10)
            {
                throw new Exception("Too much try times");
            }
        }

        //increase prize count in necessary
        //ManagePlayerPrizeLimits(prize, playerId);

        return prize;
    }

    private bool IsPrizeValid(BasePrize prize, int playerId)
    {
        if (prize == null || prize.RemainingGlobalLimit <= 0 || prize.RemainingGlobalSetLimit <= 0)
        {
            return false;
        }

        var limitedPlayerPrizeRepository = RepositoryManager.LimitedPrizeCountsByPlayerRepository();

        var limitedPlayerPrize = limitedPlayerPrizeRepository
            .Query(pp => pp.PrizeId == prize.Id && pp.PlayerId == playerId)
            .FirstOrDefault();

        if (limitedPlayerPrize?.Count >= prize.PerPlayerSetLimit)
        {
            return false;
        }

        ManagePlayerPrizeLimits(prize, playerId, limitedPlayerPrize);

        return true;
    }

    private void ManagePlayerPrizeLimits(BasePrize prize, int playerId, LimitedPrizeCountsByPlayer? limitedPlayerPrize)
    {
        var limitedPlayerPrizeRepository = RepositoryManager.LimitedPrizeCountsByPlayerRepository();

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

        //TODO: save remainings to database ...
        limitedPlayerPrizeRepository.SaveAsync();
    }

    internal abstract BasePrize GeneratePrize();
}