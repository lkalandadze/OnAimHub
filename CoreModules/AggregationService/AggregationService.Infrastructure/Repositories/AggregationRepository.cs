﻿using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Infrastructure.Persistance.Data;

namespace AggregationService.Infrastructure.Repositories;

public class AggregationRepository(AggregationServiceContext context) : BaseRepository<AggregationServiceContext, Aggregation>(context), IAggregationRepository
{
}