﻿using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllLeaderBoard;

public record GetAllLeaderBoardQuery(LeaderBoardFilter? Filter) : IQuery<ApplicationResult>;
