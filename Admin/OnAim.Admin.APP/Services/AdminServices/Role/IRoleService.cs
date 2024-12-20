﻿using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Role;

namespace OnAim.Admin.APP.Services.AdminServices.Role;

public interface IRoleService
{
    Task<ApplicationResult> Create(CreateRoleRequest request);
    Task<ApplicationResult> Delete(List<int> ids);
    Task<ApplicationResult> Update(int id, UpdateRoleRequest request);
    Task<ApplicationResult> GetAll(RoleFilter filter);
    Task<ApplicationResult> GetById(int id);
}
