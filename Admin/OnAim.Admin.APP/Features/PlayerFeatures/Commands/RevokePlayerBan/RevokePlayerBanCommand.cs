﻿using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.RevokePlayerBan;

public record RevokePlayerBanCommand(int Id) : ICommand<ApplicationResult<bool>>;
