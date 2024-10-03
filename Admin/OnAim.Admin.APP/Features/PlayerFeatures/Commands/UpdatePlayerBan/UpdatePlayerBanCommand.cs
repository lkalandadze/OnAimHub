﻿using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.UpdatePlayerBan;

public record UpdatePlayerBanCommand(int Id, DateTimeOffset? ExpireDate, bool IsPermanent, string Description) : ICommand<ApplicationResult>;