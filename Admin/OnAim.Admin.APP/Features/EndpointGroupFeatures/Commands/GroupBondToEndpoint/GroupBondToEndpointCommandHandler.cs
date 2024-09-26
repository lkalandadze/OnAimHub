using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.GroupBondToEndpoint;

public class GroupBondToEndpointCommandHandler : ICommandHandler<GroupBondToEndpointCommand, ApplicationResult>
{
    private readonly IConfigurationRepository<EndpointGroupEndpoint> _repository;
    private readonly IConfigurationRepository<RoleEndpointGroup> _roleRepository;

    public GroupBondToEndpointCommandHandler(
        IConfigurationRepository<EndpointGroupEndpoint> repository,
        IConfigurationRepository<RoleEndpointGroup> roleRepository)

    {
        _repository = repository;
        _roleRepository = roleRepository;
    }
    public async Task<ApplicationResult> Handle(GroupBondToEndpointCommand request, CancellationToken cancellationToken)
    {
        if (request.Roles != null)
        {
            foreach (var role in request.Roles)
            {
                var groupRole = await _roleRepository
                    .Query(x => x.EndpointGroupId == request.GroupId && x.RoleId == role.Id)
                    .FirstOrDefaultAsync();

                if (groupRole == null)
                {
                    throw new BadRequestException("Bad Request");
                }

                groupRole.IsActive = role.IsActive ?? true;

                await _roleRepository.CommitChanges();
            }
        }

        if (request.Endpoints != null)
        {
            foreach (var ep in request.Endpoints)
            {
                var groupEp = await _repository
                    .Query(x => x.EndpointGroupId == request.GroupId && x.EndpointId == ep.Id)
                    .FirstOrDefaultAsync();

                if (groupEp == null)
                {
                    throw new BadRequestException("Bad Request");
                }

                groupEp.IsActive = ep.IsActive;

                await _repository.CommitChanges();
            }
        }

        return new ApplicationResult { Success = true };
    }
}
