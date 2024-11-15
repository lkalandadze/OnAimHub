using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class WithdrawOptionGroupRepository(HubDbContext context) : BaseRepository<HubDbContext, WithdrawOptionGroup>(context), IWithdrawOptionGroupRepository
{
}