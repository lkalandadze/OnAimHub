namespace GameLib.Domain.Abstractions.Repository;

public interface IPrizeGroupRepository<TPrizeGroup> : IBaseRepository<TPrizeGroup>
    where TPrizeGroup : BasePrizeGroup
{
    List<TPrizeGroup> QueryWithPrizes();
}