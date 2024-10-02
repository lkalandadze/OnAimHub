using GameLib.Infrastructure.Repositories;
using Wheel.Domain.Abstractions.Repository;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.DataAccess;

namespace Wheel.Infrastructure.Repositories;

public class WheelPrizeRepository(WheelConfigDbContext context) : BaseRepository<WheelConfigDbContext, WheelPrize>(context), IWheelPrizeRepository
{
}