﻿using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Activate;

public record ActivateAccountCommand(string Email, string Code) : ICommand<ApplicationResult<string>>;