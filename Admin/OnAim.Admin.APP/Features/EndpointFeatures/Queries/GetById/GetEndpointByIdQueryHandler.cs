using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.APP.CQRS.Query;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetById;

public class GetEndpointByIdQueryHandler : IQueryHandler<GetEndpointByIdQuery, ApplicationResult>
{
    private readonly IRepository<Endpoint> _repository;
    private readonly IRepository<User> _userRepository;

    public GetEndpointByIdQueryHandler(
        IRepository<Endpoint> repository,
        IRepository<User> userRepository
        )
    {
        _repository = repository;
        _userRepository = userRepository;
    }
    public async Task<ApplicationResult> Handle(GetEndpointByIdQuery request, CancellationToken cancellationToken)
    {
        var endpoint = await _repository
            .Query(x => x.Id == request.Id)
            .Include(x => x.EndpointGroupEndpoints)
            .ThenInclude(x => x.EndpointGroup)
            .FirstOrDefaultAsync(cancellationToken);

        if (endpoint == null)
            throw new NotFoundException("Permmission Not Found!");

        var user = await _userRepository.Query(x => x.Id == endpoint.CreatedBy).FirstOrDefaultAsync();

        var result = new EndpointResponseModel
        {
            Id = request.Id,
            Name = endpoint.Name,
            Path = endpoint.Path,
            Description = endpoint.Description,
            IsActive = endpoint.IsActive,
            CreatedBy = user == null ? null : new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            },
            DateCreated = endpoint.DateCreated,
            DateDeleted = endpoint.DateDeleted,
            DateUpdated = endpoint.DateUpdated,
            Type = ToHttpMethodExtension.ToHttpMethod(endpoint.Type),
            Groups = endpoint.EndpointGroupEndpoints.Select(x => new EndpointGroupDto
            {
                Id = x.EndpointGroupId,
                Name = x.EndpointGroup.Name,
                IsActive = x.EndpointGroup.IsActive,
            }).ToList(),
        };

        return new ApplicationResult
        {
            Success = true,
            Data = result,
        };
    }
}
