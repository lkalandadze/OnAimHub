using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;

public class CreateEndpointCommandHandler : BaseCommandHandler<CreateEndpointCommand, ApplicationResult>
{
    private readonly IRepository<Endpoint> _repository;

    public CreateEndpointCommandHandler(
        CommandContext<CreateEndpointCommand> context,
        IRepository<Endpoint> repository)
        : base(context)
    {
        _repository = repository;
    }

    protected override async Task<ApplicationResult> ExecuteAsync(CreateEndpointCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var existedEndpoint = await _repository.Query(x => x.Name == request.Name).FirstOrDefaultAsync();

        EndpointType endpointTypeEnum = EndpointType.Get;

        if (request.Type != null && Enum.TryParse(request.Type, true, out EndpointType parsedType))
            endpointTypeEnum = parsedType;

        if (existedEndpoint != null)
            throw new AlreadyExistsException("Permission with that name already exists.");

        var endpoint = new Endpoint(
            request.Name,
            request.Name,
            _context.SecurityContextAccessor.UserId,
            endpointTypeEnum,
            request.Description ?? "Description needed"
        );

        await _repository.Store(endpoint);
        await _repository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"Permission {endpoint.Name} Created",
        };
    }
}
