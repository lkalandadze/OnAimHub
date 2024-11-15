using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class WithdrawOptionRepository(HubDbContext context) : BaseRepository<HubDbContext, WithdrawOption>(context), IWithdrawOptionRepository
{
}