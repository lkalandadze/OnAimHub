﻿using OnAim.Admin.Contracts.Dtos.EndpointGroup;

namespace OnAim.Admin.APP.Services.AdminServices.EndpointGroup;

public interface IEndpointGroupService
{
    Task<ApplicationResult<string>> Create(CreateEndpointGroupRequest model);
    Task<ApplicationResult<bool>> Delete(List<int> ids);
    Task<ApplicationResult<string>> Update(int id, UpdateEndpointGroupRequest model);
    Task<ApplicationResult<PaginatedResult<EndpointGroupModel>>> GetAll(EndpointGroupFilter filter);
    Task<ApplicationResult<EndpointGroupResponseDto>> GetById(int id);
}