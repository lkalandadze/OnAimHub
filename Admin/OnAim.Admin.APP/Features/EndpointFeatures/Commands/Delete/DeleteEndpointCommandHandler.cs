using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;

public class DeleteEndpointCommandHandler : BaseCommandHandler<DeleteEndpointCommand, ApplicationResult>
{
    private readonly IRepository<Endpoint> _repository;

    public DeleteEndpointCommandHandler(
        CommandContext<DeleteEndpointCommand> context,
        IRepository<Endpoint> repository
        ) : base(context)
    {
        _repository = repository;
    }

    protected override async Task<ApplicationResult> ExecuteAsync(DeleteEndpointCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var endpoints = await _repository.Query(x => request.Ids.Contains(x.Id)).ToListAsync();

        if (!endpoints.Any())
            throw new NotFoundException("Permission Not Found");

        foreach ( var endpoint in endpoints )
        {
            endpoint.IsActive = false;
            endpoint.IsDeleted = true;
        }

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}
