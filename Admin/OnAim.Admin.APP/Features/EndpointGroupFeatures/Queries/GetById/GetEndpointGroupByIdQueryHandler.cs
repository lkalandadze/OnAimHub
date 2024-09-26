using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.DTOs.User;
namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetById;

public class GetEndpointGroupByIdQueryHandler : IQueryHandler<GetEndpointGroupByIdQuery, ApplicationResult>
{
    private readonly IRepository<Domain.Entities.EndpointGroup> _repository;
    private readonly IRepository<Domain.Entities.User> _userRepo;

    public GetEndpointGroupByIdQueryHandler(
        IRepository<Domain.Entities.EndpointGroup> repository,
        IRepository<Domain.Entities.User> userRepo
        )
    {
        _repository = repository;
        _userRepo = userRepo;
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

        var user = await _userRepo.Query(x => x.Id == group.CreatedBy).FirstOrDefaultAsync();

        if (group == null)
        {
            throw new NotFoundException("Permmission Group Not Found!");
        }

        var res = new EndpointGroupResponseDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            IsActive = group.IsActive,
            DateCreated = group.DateCreated,
            DateDeleted = group.DateDeleted,
            DateUpdated = group.DateUpdated,
            CreatedBy = user == null ? null : new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            },
            Roles = group.RoleEndpointGroups.Select(z => new RoleDto
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
