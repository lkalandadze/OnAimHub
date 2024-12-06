using GameLib.Domain.Abstractions.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using TestWheel.Domain.Entities;

namespace TestWheel.Domain.Abstractions.Repository;

public interface ITestWheelConfigurationRepository : IBaseRepository<TestWheelConfiguration>
{
    DbContext GetDbContext();
    void UpdateEntity(TestWheelConfiguration existingEntity, TestWheelConfiguration updatedEntity);
    void UpdateCollection(IEnumerable existingCollection, IEnumerable updatedCollection, Type collectionType);
    bool IsCollection(Type type);
}