using GameLib.Domain.Abstractions.Repository;
using Microsoft.EntityFrameworkCore;
using Wheel.Domain.Entities;

namespace Wheel.Domain.Abstractions.Repository;

public interface IWheelConfigurationRepository : IBaseRepository<WheelConfiguration>
{
    DbContext GetDbContext();
}