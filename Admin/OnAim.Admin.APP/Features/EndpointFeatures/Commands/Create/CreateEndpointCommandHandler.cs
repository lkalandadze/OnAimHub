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

    public CreateEndpointCommandHandler(CommandContext<CreateEndpointCommand> context,IRepository<Endpoint> repository): base(context)
    {
        _repository = repository;
    }

    protected override async Task<ApplicationResult> ExecuteAsync(CreateEndpointCommand request, CancellationToken cancellationToken)
    {
        var results = new List<Endpoint>();

        foreach (var endpointDto in request.Endpoints)
        {
            await ValidateAsync(request, cancellationToken);

            var existedEndpoint = await _repository.Query(x => x.Name == endpointDto.Name).FirstOrDefaultAsync(cancellationToken);

            EndpointType endpointTypeEnum = EndpointType.Get;

            if (endpointDto.Type != null && Enum.TryParse(endpointDto.Type, true, out EndpointType parsedType))
                endpointTypeEnum = parsedType;

            if (existedEndpoint != null)
                throw new BadRequestException($"Endpoint with name '{endpointDto.Name}' already exists.");

            var endpoint = new Endpoint(
                endpointDto.Name,
                endpointDto.Name,
                _context.SecurityContextAccessor.UserId,
                endpointTypeEnum,
                endpointDto.Description ?? "Description needed"
            );

            await _repository.Store(endpoint);
            results.Add(endpoint);
        }

        await _repository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = string.Join(", ", results.Select(x => x.Name)),
        };
    }
}
