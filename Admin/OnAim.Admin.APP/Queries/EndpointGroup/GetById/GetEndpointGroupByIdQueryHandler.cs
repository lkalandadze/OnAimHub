﻿using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Models.Response.EndpointGroupResponseModels;
using OnAim.Admin.APP.Models.Response.EndpointModels;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetById
{
    public class GetEndpointGroupByIdQueryHandler : IQueryHandler<GetEndpointGroupByIdQuery, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _repository;

        public GetEndpointGroupByIdQueryHandler(IRepository<Infrasturcture.Entities.EndpointGroup> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(GetEndpointGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var group = await _repository
                .Query(x => x.Id == request.Id)
                .Include(x => x.RoleEndpointGroups)
                .ThenInclude(x => x.Role)
                .Include(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint)
                .FirstOrDefaultAsync();

            if (group == null)
            {
                return new ApplicationResult { Success = false, Data = $"Permmission Group Not Found!" };
            }

            var res = new EndpointGroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                IsActive = group.IsActive,
                DateCreated = group.DateCreated,
                DateDeleted = group.DateDeleted,
                DateUpdated = group.DateUpdated,
                Roles = group.RoleEndpointGroups.Select(z => new Models.Response.User.RoleDto
                {
                    Id = z.Role.Id,
                    Name = z.Role.Name,
                    IsActive = z.Role.IsActive,
                }).ToList(),
                Endpoints = group.EndpointGroupEndpoints.Select(x => new EndpointModel
                {
                    Id = x.Endpoint.Id,
                    Name = x.Endpoint.Name,
                    Description = x.Endpoint.Description,
                    IsActive = x.Endpoint.IsActive,
                    IsEnabled = x.Endpoint.IsDeleted,
                }).ToList()
            };

            return new ApplicationResult
            {
                Success = true,
                Data = res,
            };
        }
    }
}
