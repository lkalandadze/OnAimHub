﻿using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById;

public record GetGameQuery(int Id) : IQuery<ApplicationResult>;