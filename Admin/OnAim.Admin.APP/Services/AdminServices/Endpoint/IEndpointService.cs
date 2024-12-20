﻿using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Endpoint;

namespace OnAim.Admin.APP.Services.AdminServices.Endpoint;

public interface IEndpointService
{
    Task<ApplicationResult> Create(List<CreateEndpointDto> endpoints);
    Task<ApplicationResult> Delete(List<int> ids);
    Task<ApplicationResult> Update(int id, UpdateEndpointDto endpoint);
    Task<ApplicationResult> GetAll(EndpointFilter filter);
    Task<ApplicationResult> GetById(int id);
}