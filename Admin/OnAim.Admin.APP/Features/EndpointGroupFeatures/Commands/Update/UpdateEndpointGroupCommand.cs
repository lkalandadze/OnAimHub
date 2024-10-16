﻿using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EndpointGroup;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Update;

public record UpdateEndpointGroupCommand(int Id, UpdateEndpointGroupRequest Model) : ICommand<ApplicationResult>;
