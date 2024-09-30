using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Domain.Exceptions;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Delete;

public class DeleteEndpointGroupCommandHandler : BaseCommandHandler<DeleteEndpointGroupCommand, ApplicationResult>
{
    private readonly IRepository<EndpointGroup> _repository;

    public DeleteEndpointGroupCommandHandler(CommandContext<DeleteEndpointGroupCommand> context,IRepository<EndpointGroup> repository) : base( context )
    {
        _repository = repository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(DeleteEndpointGroupCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var groups = await _repository.Query(x => request.GroupIds.Contains(x.Id)).ToListAsync();

        if (!groups.Any())
            throw new NotFoundException("Permission Group Not Found");

        foreach ( var group in groups)
        {
            group.IsActive = false;
            group.MarkAsDeleted();
        }

        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }
}
