using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class WithdrawOptionEndpointRepository(HubDbContext context) : BaseRepository<HubDbContext, WithdrawOptionEndpoint>(context), IWithdrawOptionEndpointRepository
{
}