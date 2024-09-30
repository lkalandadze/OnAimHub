using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Helpers;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, ApplicationResult>
{
    private readonly IRepository<User> _repository;
    private readonly IConfigurationRepository<AuditLog> _configurationRepository;

    public GetUserByIdQueryHandler(
        IRepository<User> repository,
        IConfigurationRepository<AuditLog> configurationRepository
        )
    {
        _repository = repository;
        _configurationRepository = configurationRepository;
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

        if (query == null) { return new ApplicationResult { Success = false, Data = $"User Not Found!" }; }

        var logs = await _configurationRepository.Query(x => x.UserId == query.Id).ToListAsync();

        var usert = await _repository.Query(x => x.Id == query.CreatedBy).FirstOrDefaultAsync();

        var user = new GetUserModel
        {
            Id = query.Id,
            FirstName = query.FirstName,
            LastName = query.LastName,
            Email = query.Email,
            Phone = query.Phone,
            IsActive = query.IsActive,
            UserPreferences = query.Preferences ?? new UserPreferences(),
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
                        Path = xxx.Endpoint.Path,
                        Description = xxx.Endpoint.Description,
                        Type = ToHttpMethodExtension.ToHttpMethod(xxx.Endpoint.Type),
                        IsActive = xxx.Endpoint.IsActive,
                        IsEnabled = xxx.Endpoint.IsDeleted,
                        DateCreated = xxx.Endpoint.DateCreated,
                    }).ToList(),
                }).ToList()
            }).ToList(),
            CreatedBy = usert == null ? null : new UserDto
            {
                Id = usert.Id,
                FirstName = usert.FirstName,
                LastName = usert.LastName,
                Email = usert.Email,
            },
            Logs = logs.Select(x => new LogDto
            {
                Action = x.Action,
                Log = x.Log,
                DateCreated = x.Timestamp
            }).ToList(),
        };

        return new ApplicationResult
        {
            Success = true,
            Data = user,
        };
    }
}
