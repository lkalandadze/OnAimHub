using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Models.Response.UserResponseModels;
using OnAim.Admin.Infrasturcture.Extensions;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.User.GetById
{
    public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;

        public GetUserByIdQueryHandler(IRepository<Infrasturcture.Entities.User> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository
                .Query(x => x.Id == request.Id)
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.RoleEndpointGroups)
                .ThenInclude(x => x.EndpointGroup)
                .ThenInclude(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint)
                .FirstOrDefaultAsync();

            var user = new GetUserModel
            {
                Id = query.Id,
                FirstName = query.FirstName,
                LastName = query.LastName,
                Email = query.Email,
                Phone = query.Phone,
                IsActive = query.IsActive,
                Roles = query.UserRoles.Select(x => new RoleModel
                {
                    Id = x.RoleId,
                    Name = x.Role.Name,
                    IsActive = x.Role.IsActive,
                    EndpointGroupModels = x.Role.RoleEndpointGroups.Select(xx => new EndpointGroupDto
                    {
                        Id = xx.EndpointGroupId,
                        Name = xx.EndpointGroup.Name,
                        Description = xx.EndpointGroup.Description,
                        IsActive = xx.EndpointGroup.IsActive,
                        Endpoints = xx.EndpointGroup.EndpointGroupEndpoints.Select(xxx => new EndpointRequestModel
                        {
                            Id = xxx.Endpoint.Id,
                            Name = xxx.Endpoint.Name,
                            Description = xxx.Endpoint.Description,
                            Type = ToHttpMethodExtension.ToHttpMethod(xxx.Endpoint.Type),
                            IsActive = xxx.Endpoint.IsActive,
                            IsEnabled = xxx.Endpoint.IsEnabled,
                            DateCreated = xxx.Endpoint.DateCreated,
                        }).ToList(),
                    }).ToList()
                }).ToList(),
            };

            return new ApplicationResult
            {
                Success = true,
                Data = user,
            };
        }
    }
}
