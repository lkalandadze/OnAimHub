﻿using Hub.Domain.Entities.Templates;
using Shared.Domain.Abstractions.Repository;

namespace Hub.Domain.Abstractions.Repository;

public interface ICoinTemplateRepository : IBaseEntityRepository<CoinTemplate>
{
}