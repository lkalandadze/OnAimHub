using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.AdminServices.Endpoint;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;

public class CreateEndpointCommandHandler : ICommandHandler<CreateEndpointCommand, ApplicationResult<string>>
{
    private readonly IEndpointService _permissionService;
    private readonly IValidator<CreateEndpointCommand> _validator;

    public CreateEndpointCommandHandler(IEndpointService permissionService, IValidator<CreateEndpointCommand> validator)
    {
        _permissionService = permissionService; 
        _validator = validator;
    }

    public async Task<ApplicationResult<string>> Handle(CreateEndpointCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _permissionService.Create(request.Endpoints);
    }
}
