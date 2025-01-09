using GameLib.Infrastructure.Repositories;
using PenaltyKicks.Domain.Abstractions.Repository;
using PenaltyKicks.Domain.Entities;
using PenaltyKicks.Infrastructure.DataAccess;

namespace PenaltyKicks.Infrastructure.Repositories;

public class PenaltyConfigurationRepository(PenaltyConfigDbContext context) : BaseRepository<PenaltyConfigDbContext, PenaltyConfiguration>(context), IPenaltyConfigurationRepository
{
}