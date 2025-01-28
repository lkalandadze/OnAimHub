using GameLib.Application.Managers;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;

namespace GameLib.Application.Generators;

internal abstract class Generator
{
    private ILimitedPrizeCountsByPlayerRepository _limitedPlayerPrizeRepository;

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
        _limitedPlayerPrizeRepository = RepositoryManager.LimitedPrizeCountsByPlayerRepository();
        var maxAttempts = Prizes.Count + (Prizes.Count / 2);

        for (int i = 0; i < maxAttempts; i++)
        {
            var prize = GeneratePrize();
            var limitedPlayerPrize = GetLimitedPlayerPrize(prize.Id, playerId);

            if (IsPrizeValid(prize, playerId, limitedPlayerPrize))
            {
                UpdateRemainingGlobalLimits(prize);
                UpdatePlayerPrizeLimits(prize, playerId, limitedPlayerPrize);

                return prize;
            }
        }
        
        throw new Exception("Exceeded maximum retry attempts to get a valid prize");
    }

    private bool IsPrizeValid(BasePrize prize, int playerId, LimitedPrizeCountsByPlayer? limitedPlayerPrize)
    {
        if (prize == null) //null check
        {
            return false;
        }

        if (prize.RemainingGlobalLimit <= 0 || limitedPlayerPrize?.Count >= prize.PerPlayerLimit) //global limit checks
        {
            return false;
        }

        if (prize.RemainingGlobalSetLimit <= 0 || limitedPlayerPrize?.Count >= prize.PerPlayerSetLimit * prize.SetSize) //set limit checks
        {
            return false;
        }

        return true;
    }

    private void UpdatePlayerPrizeLimits(BasePrize prize, int playerId, LimitedPrizeCountsByPlayer? limitedPlayerPrize)
    {
        if (limitedPlayerPrize != null)
        {
            limitedPlayerPrize.IncreaseCount();
            _limitedPlayerPrizeRepository.Update(limitedPlayerPrize);
        }
        else
        {
            var newLimitedPrize = new LimitedPrizeCountsByPlayer(playerId, prize.Id);
            newLimitedPrize.IncreaseCount();
            _limitedPlayerPrizeRepository.InsertAsync(newLimitedPrize);
        }

        _limitedPlayerPrizeRepository.SaveAsync();
    }

    private void UpdateRemainingGlobalLimits(BasePrize prize)
    {
        var totalSetWinCount = _limitedPlayerPrizeRepository.Query(pp => pp.PrizeId == prize.Id && pp.Count >= prize.SetSize)
                                                            .Count();

        var latestRemainingSetLimit = prize.GlobalSetLimit - totalSetWinCount;

        if (prize.RemainingGlobalSetLimit > latestRemainingSetLimit)
        {
            prize.UpdateRemainingGlobalSetLimit(latestRemainingSetLimit);
        }

        prize.DecrementRemainingGlobalLimit();

        var repo = RepositoryManager.PrizeRepository(prize.GetType());
        repo.Update(prize);

        //TODO: RemainingGlobalSetLimit & RemainingGlobalLimit - save to database (Prize table)
    }

    private LimitedPrizeCountsByPlayer? GetLimitedPlayerPrize(int prizeId, int playerId)
    {
        return _limitedPlayerPrizeRepository.Query(pp => pp.PrizeId == prizeId && pp.PlayerId == playerId).FirstOrDefault();
    }

    internal abstract BasePrize GeneratePrize();
}