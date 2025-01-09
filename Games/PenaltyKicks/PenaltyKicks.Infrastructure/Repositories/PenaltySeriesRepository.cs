using GameLib.Infrastructure.Repositories;
using PenaltyKicks.Domain.Abstractions.Repository;
using PenaltyKicks.Domain.Entities;
using PenaltyKicks.Infrastructure.DataAccess;

namespace PenaltyKicks.Infrastructure.Repositories;

public class PenaltySeriesRepository(PenaltyConfigDbContext context) : BaseRepository<PenaltyConfigDbContext, PenaltySeries>(context), IPenaltySeriesRepository
{
}