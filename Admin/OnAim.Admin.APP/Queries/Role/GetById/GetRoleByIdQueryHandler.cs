using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Extensions;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup;
using OnAim.Admin.Infrasturcture.Models.Response.Role;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.Role.GetById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.Role> _repository;

        public GetRoleByIdQueryHandler(IRepository<Infrasturcture.Entities.Role> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _repository
                .Query(x => x.Id == request.Id)
                .Include(x => x.RoleEndpointGroups)
                .ThenInclude(x => x.EndpointGroup)
                .ThenInclude(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint)
                .FirstOrDefaultAsync();

            var result = new RoleResponseModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                DateCreated = role.DateCreated,
                EndpointGroupModels = role.RoleEndpointGroups.Select(x => new EndpointGroupModel
                {
                    Id = x.EndpointGroupId,
                    Name = x.EndpointGroup.Name,
                    IsActive = x.EndpointGroup.IsActive,
                    Description = x.EndpointGroup.Description,
                    DateCreated = x.EndpointGroup.DateCreated,
                    Endpoints = x.EndpointGroup.EndpointGroupEndpoints.Select(x => new EndpointRequestModel
                    {
                        Id = x.EndpointId,
                        Name = x.Endpoint.Name,
                        Description = x.Endpoint.Description,
                        IsActive = x.Endpoint.IsActive,
                        IsEnabled = x.Endpoint.IsEnabled,
                        Type = ToHttpMethodExtension.ToHttpMethod(x.Endpoint.Type),
                        Path = x.Endpoint.Path,
                        DateCreated = x.Endpoint.DateCreated,
                    }).ToList(),
                }).ToList(),
            };

            return new ApplicationResult
            {
                Success = true,
                Data = result,
            };
        }
    }
}
