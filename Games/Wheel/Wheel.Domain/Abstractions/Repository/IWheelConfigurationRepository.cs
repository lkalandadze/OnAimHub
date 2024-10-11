using GameLib.Domain.Abstractions.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Wheel.Domain.Entities;

namespace Wheel.Domain.Abstractions.Repository;

public interface IWheelConfigurationRepository : IBaseRepository<WheelConfiguration>
{
    DbContext GetDbContext();
    void UpdateEntity(WheelConfiguration existingEntity, WheelConfiguration updatedEntity);
    void UpdateCollection(IEnumerable existingCollection, IEnumerable updatedCollection, Type collectionType);
    bool IsCollection(Type type);
}