﻿using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class SegmentRepository(HubDbContext context) : BaseRepository<HubDbContext, Segment>(context), ISegmentRepository
{
}