using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class WithdrawOptionRepository(HubDbContext context) : BaseRepository<HubDbContext, WithdrawOption>(context), IWithdrawOptionRepository
{
}