﻿using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.BanPlayer;

public sealed record BanPlayerCommand(int PlayerId, DateTimeOffset? ExpireDate, bool IsPermanent, string Description) : ICommand<ApplicationResult<bool>>;