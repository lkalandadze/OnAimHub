using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class WithdrawEndpointTemplateRepository(HubDbContext context) : BaseRepository<HubDbContext, WithdrawEndpointTemplate>(context), IWithdrawEndpointTemplateRepository
{
}