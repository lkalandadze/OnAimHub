﻿using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Disable
{
    public record DisableEndpointCommand(int EndpointId) : IRequest<ApplicationResult>;
}