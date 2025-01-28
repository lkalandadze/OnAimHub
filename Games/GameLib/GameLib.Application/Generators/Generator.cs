using GameLib.Application.Managers;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
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
        int attempts = 0;

        do
        {
            var prize = GeneratePrize();
            var limitedPlayerPrize = GetLimitedPlayerPrize(prize.Id, playerId);

            if (IsPrizeValid(prize, playerId, limitedPlayerPrize))
            {
                UpdatePlayerPrizeLimits(prize, playerId, limitedPlayerPrize);
                return prize;
            }

            attempts++;
        }
        while (attempts < 15);

        throw new Exception("Exceeded maximum retry attempts to get a valid prize");
    }

    private bool IsPrizeValid(BasePrize prize, int playerId, LimitedPrizeCountsByPlayer? limitedPlayerPrize)
    {
        if (prize == null ||
            prize.RemainingGlobalLimit <= 0 ||
            prize.RemainingGlobalSetLimit <= 0)
        {
            return false;
        }

        if (limitedPlayerPrize?.Count >= prize.PerPlayerSetLimit)
        {
            return false;
        }

        return true;
    }

    private void UpdatePlayerPrizeLimits(BasePrize prize, int playerId, LimitedPrizeCountsByPlayer? limitedPlayerPrize)
    {
        var repository = RepositoryManager.LimitedPrizeCountsByPlayerRepository();

        prize.DecrementRemainingGlobalLimit();

        if (limitedPlayerPrize != null)
        {
            limitedPlayerPrize.IncreaseCount();
            repository.Update(limitedPlayerPrize);
        }
        else
        {
            var newLimitedPrize = new LimitedPrizeCountsByPlayer(playerId, prize.Id);
            newLimitedPrize.IncreaseCount();
            repository.InsertAsync(newLimitedPrize);
        }

        UpdateGlobalSetLimit(prize, playerId, repository);

        //TODO: RemainingGlobalLimit - save to database

        repository.SaveAsync();
    }

    private void UpdateGlobalSetLimit(BasePrize prize, int playerId, ILimitedPrizeCountsByPlayerRepository repository)
    {
        var totalSetWinCount = repository.Query(pp => pp.PrizeId == prize.Id && pp.Count >= prize.SetSize)
                                         .Count();

        var remainingSetLimit = prize.GlobalSetLimit - totalSetWinCount;

        if (remainingSetLimit < prize.RemainingGlobalSetLimit)
        {
            prize.RemainingGlobalSetLimit = remainingSetLimit;
        }

        //TODO: RemainingGlobalSetLimit - save to database
    }

    private LimitedPrizeCountsByPlayer? GetLimitedPlayerPrize(int prizeId, int playerId)
    {
        var repository = RepositoryManager.LimitedPrizeCountsByPlayerRepository();
        return repository.Query(pp => pp.PrizeId == prizeId && pp.PlayerId == playerId).FirstOrDefault();
    }

    internal abstract BasePrize GeneratePrize();
}