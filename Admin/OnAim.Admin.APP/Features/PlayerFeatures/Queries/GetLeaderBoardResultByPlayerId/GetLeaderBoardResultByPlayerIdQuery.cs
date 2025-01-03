﻿using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetLeaderBoardResultByPlayerId;

public record GetLeaderBoardResultByPlayerIdQuery(int PlayerId) : IQuery<ApplicationResult>;
