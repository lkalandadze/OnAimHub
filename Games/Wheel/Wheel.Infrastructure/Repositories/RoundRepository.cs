using GameLib.Infrastructure.Repositories;
using Wheel.Domain.Abstractions.Repository;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.DataAccess;

namespace Wheel.Infrastructure.Repositories;

public class RoundRepository(WheelConfigDbContext context) : BaseRepository<WheelConfigDbContext, Round>(context), IRoundRepository
{
}