using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Helpers;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.GetById;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, ApplicationResult>
{
    private readonly IRepository<Domain.Entities.Role> _repository;
    private readonly IRepository<Domain.Entities.User> _userRepository;

    public GetRoleByIdQueryHandler(
        IRepository<Domain.Entities.Role> repository,
        IRepository<Domain.Entities.User> userRepository
        )
    {
        _repository = repository;
        _userRepository = userRepository;
    }
    public async Task<ApplicationResult> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _repository
            .Query(x => x.Id == request.Id)
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.User)
            .Include(x => x.RoleEndpointGroups)
            .ThenInclude(x => x.EndpointGroup)
            .ThenInclude(x => x.EndpointGroupEndpoints)
            .ThenInclude(x => x.Endpoint)
            .FirstOrDefaultAsync(cancellationToken);

        var user = await _userRepository.Query(x => x.Id == role.CreatedBy).FirstOrDefaultAsync(cancellationToken);

        if (role == null) { return new ApplicationResult { Success = false, Data = $"Role Not Found!" }; }

        var result = new RoleResponseModel
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            DateCreated = role.DateCreated,
            UsersResponseModels = role.UserRoles.Select(z => new UserDto
            {
                Id = z.UserId,
                FirstName = z.User.FirstName,
                LastName = z.User.LastName,
                Email = z.User.Email,
            }).ToList(),
            EndpointGroupModels = role.RoleEndpointGroups.Select(x => new EndpointGroupModel
            {
                Id = x.EndpointGroupId,
                Name = x.EndpointGroup.Name,
                IsActive = x.EndpointGroup.IsActive,
                Description = x.EndpointGroup.Description,
                DateCreated = x.EndpointGroup.DateCreated,
                EndpointsCount = x.EndpointGroup.EndpointGroupEndpoints.Count,
                Endpoints = x.EndpointGroup.EndpointGroupEndpoints.Select(x => new EndpointRequestModel
                {
                    Id = x.EndpointId,
                    Name = x.Endpoint.Name,
                    Description = x.Endpoint.Description,
                    IsActive = x.Endpoint.IsActive,
                    IsEnabled = x.Endpoint.IsDeleted,
                    Type = ToHttpMethodExtension.ToHttpMethod(x.Endpoint.Type),
                    Path = x.Endpoint.Path,
                    DateCreated = x.Endpoint.DateCreated,
                }).ToList(),
            }).ToList(),
            CreatedBy = user == null ? null : new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            }
        };

        return new ApplicationResult
        {
            Success = true,
            Data = result,
        };
    }
}
