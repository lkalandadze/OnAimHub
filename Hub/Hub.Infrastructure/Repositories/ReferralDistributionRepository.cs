using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class ReferralDistributionRepository(HubDbContext context) : BaseRepository<HubDbContext, ReferralDistribution>(context), IReferralDistributionRepository
{
}