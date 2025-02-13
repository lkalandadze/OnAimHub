﻿using Hub.Domain.Entities;
using Shared.Domain.Abstractions.Repository;

namespace Hub.Domain.Abstractions.Repository;

public interface IServiceRepository : IBaseEntityRepository<Service>
{
    Task<int> InsertAndSaveAsync(Service aggregateRoot);
}