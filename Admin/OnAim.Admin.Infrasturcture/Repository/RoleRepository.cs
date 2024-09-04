using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup;
using OnAim.Admin.Infrasturcture.Models.Response.Role;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.Infrasturcture.Repository
{
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
                .Where(role => role.IsActive == true)
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.User)
                    .Where(userRole => userRole.IsActive == true)
                .Include(r => r.RoleEndpointGroups)
                    .ThenInclude(reg => reg.EndpointGroup)
                        .ThenInclude(eg => eg.EndpointGroupEndpoints)
                            .ThenInclude(ege => ege.Endpoint)
                            .Where(endpoint => endpoint.IsActive == true)
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
}
