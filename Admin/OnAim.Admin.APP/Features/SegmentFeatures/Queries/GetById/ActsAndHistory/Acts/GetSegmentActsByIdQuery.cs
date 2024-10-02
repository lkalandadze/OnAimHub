﻿using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.ActsAndHistory.Acts;

public record GetSegmentActsByIdQuery(string SegmentId) : IQuery<ApplicationResult>;
