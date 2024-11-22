using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.Templates;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class WithdrawEndpointTemplateRepository(HubDbContext context) : BaseRepository<HubDbContext, WithdrawEndpointTemplate>(context), IWithdrawEndpointTemplateRepository
{
}