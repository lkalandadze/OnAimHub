﻿using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class SegmentRepository(HubDbContext context) : BaseRepository<HubDbContext, Segment>(context), ISegmentRepository
{
}