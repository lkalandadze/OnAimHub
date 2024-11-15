using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class WithdrawOptionGroupMappingRepository(HubDbContext context) : BaseRepository<HubDbContext, WithdrawOptionGroupMapping>(context), IWithdrawOptionGroupMappingRepository
{
}