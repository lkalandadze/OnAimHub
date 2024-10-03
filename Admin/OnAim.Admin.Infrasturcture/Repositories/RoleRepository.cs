using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.Infrasturcture.Repository;

public class RoleRepository : IRoleRepository
{
    private readonly DatabaseContext _databaseContext;

    public RoleRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    //For permission check
    public async Task<PaginatedResult<RoleResponseModel>> GetAllRoles()
    {
        var query = _databaseContext.Roles
                .Where(role => role.IsActive && !role.IsDeleted) 
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.User)
                    .Where(userRole => userRole.IsActive)
                .Include(r => r.RoleEndpointGroups)
                    .ThenInclude(reg => reg.EndpointGroup)
                        .ThenInclude(eg => eg.EndpointGroupEndpoints)
                            .ThenInclude(ege => ege.Endpoint)
                .Where(role => role.RoleEndpointGroups
                    .Any(reg => reg.EndpointGroup.EndpointGroupEndpoints
                        .Any(ege => !ege.Endpoint.IsDeleted && ege.Endpoint.IsActive))
                )
                .AsQueryable();

        var roles = await query
            .OrderBy(x => x.Id)
            .Select(x => new RoleResponseModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive,
                EndpointGroupModels = x.RoleEndpointGroups.Select(z => new EndpointGroupModel
                {
                    Id = z.EndpointGroupId,
                    Name = z.EndpointGroup.Name,
                    Description = z.EndpointGroup.Description,
                    Endpoints = z.EndpointGroup.EndpointGroupEndpoints.Select(u => new EndpointRequestModel
                    {
                        Id = u.EndpointId,
                        Name = u.Endpoint.Name,
                        Path = u.Endpoint.Path,
                        Description = u.Endpoint.Description,
                    }).ToList()
                }).ToList()
            }).ToListAsync();

        return new PaginatedResult<RoleResponseModel>
        {
            Items = roles
        };
    }
}
