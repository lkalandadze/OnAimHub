namespace GameLib.Domain.Abstractions.Repository;

public interface IPrizeRepository<TPrize> : IBaseRepository<TPrize>
    where TPrize : BasePrize
{
}