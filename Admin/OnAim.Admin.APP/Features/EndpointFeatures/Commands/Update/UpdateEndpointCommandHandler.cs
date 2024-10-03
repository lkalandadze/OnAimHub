using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Update;

public sealed class UpdateEndpointCommandHandler : BaseCommandHandler<UpdateEndpointCommand, ApplicationResult>
{
    private readonly IRepository<Endpoint> _repository;

    public UpdateEndpointCommandHandler(CommandContext<UpdateEndpointCommand> context, IRepository<Endpoint> repository) : base( context )
    {
        _repository = repository;
    }
    protected override async Task<ApplicationResult> ExecuteAsync(UpdateEndpointCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var ep = await _repository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();

        if (ep == null || ep?.IsDeleted == true)
            throw new NotFoundException("Permission Not Found");

        ep.Description = request.Endpoint.Description;
        ep.IsActive = request.Endpoint.IsActive ?? true;

        await _repository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"Permmission {ep.Name} Successfully Updated"
        };
    }
}
